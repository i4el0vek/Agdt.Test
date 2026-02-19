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

		var patients = _patientRepository.GetAllPatientsAsync();

		// TODO: Move to another place or library (including FhirDateFilter)
		foreach (var filterRaw in dateFilters.Where(x => !string.IsNullOrEmpty(x)))
		{
			var filter = FhirDateFilter.Parse(filterRaw);

			patients = filter.Prefix switch
			{
				"eq" => patients.Where(x => x.BirthDate == filter.Date),
				"ne" => patients.Where(x => x.BirthDate != filter.Date),
				"gt" => patients.Where(x => x.BirthDate > filter.Date),
				"ge" => patients.Where(x => x.BirthDate >= filter.Date),
				"lt" => patients.Where(x => x.BirthDate < filter.Date),
				"le" => patients.Where(x => x.BirthDate <= filter.Date),
				_ => patients
			};
		}

		return patients.AsAsyncEnumerable();
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
