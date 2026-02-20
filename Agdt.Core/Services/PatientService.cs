using Agdt.Core.Helpers;
using Agdt.Core.Interfaces;
using Agdt.Core.Models;

namespace Agdt.Core.Services;

public class PatientService
{
	private readonly IPatientRepository _patientRepository;
	public PatientService(IPatientRepository patientRepository)
	{
		_patientRepository = patientRepository;
	}

	public IAsyncEnumerable<Patient> GetAllPatients()
	{
		return _patientRepository.GetAllPatientsAsync();
	}

	public IAsyncEnumerable<Patient> GetAllPatientsByBirthDateAsync(string[]? dateFilters)
	{
		if (dateFilters == null || !dateFilters.Any())
		{
			return _patientRepository.GetAllPatientsAsync();
		}

		IQueryable<Patient> patients = _patientRepository.GetAllPatientsQueryAsync();

		// TODO: Move to another place or library (including FhirDateFilter)
		var filters = dateFilters
			.Where(x => !string.IsNullOrEmpty(x))
			.Select(FhirDateFilter.Parse)
			.ToList();

		foreach (var filter in filters)
		{
			patients = filter.Prefix switch
			{
				"eq" => patients.Where(x => x.BirthDate == filter.Date), // does not match the specs
				"ne" => patients.Where(x => x.BirthDate != filter.Date), // does not match the specs
				"lt" => patients.Where(x => x.BirthDate < filter.Date),
				"gt" => patients.Where(x => x.BirthDate > filter.Date),
				"ge" => patients.Where(x => x.BirthDate >= filter.Date),
				"le" => patients.Where(x => x.BirthDate <= filter.Date),
				_ => patients // sa/eb/ap/text format - too much, left it as is
			};
		}

		return patients.ToAsyncEnumerable();
	}

	public async Task<Patient?> GetPatientAsync(Guid id)
	{
		return await _patientRepository.GetPatientAsync(id);
	}

	public async Task<Guid> RegisterAsync(Patient patient)
	{
		var id = await _patientRepository.AddPatientAsync(patient);
		await _patientRepository.SaveChangesAsync();
		return id;
	}

	public async Task<bool> UdpateAsync(Patient patient)
	{
		if (await _patientRepository.GetPatientAsync(patient.Id) == null)
		{
			return false;
		}

		await _patientRepository.UdpatePatientAsync(patient);
		await _patientRepository.SaveChangesAsync();
		return true;
	}

	public async Task<bool> DeregisterAsync(Guid id)
	{
		if (await _patientRepository.GetPatientAsync(id) == null)
		{
			return false;
		}

		await _patientRepository.DeletePatientAsync(id);
		await _patientRepository.SaveChangesAsync();
		return true;
	}
}
