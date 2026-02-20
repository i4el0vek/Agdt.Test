using Agdt.Core.Interfaces;
using Agdt.Core.Models;
using Agdt.Persistent.Entities;
using Agdt.Persistent.Services;
using Microsoft.EntityFrameworkCore;

namespace Agdt.Persistent.Repositories;

public class PatientRepository : IPatientRepository
{
	private readonly PatientDbContext _patientDbContext;
	public PatientRepository(PatientDbContext patientDbContext)
	{
		_patientDbContext = patientDbContext;
	}

	public async Task<Guid> AddPatientAsync(Patient patient)
	{
		var model = new PatientEntity()
		{
			Name = new NameEntity()
			{
				Family = patient.Name.Family,
				Given = patient.Name.Given,
				Use = patient.Name.Use,
			},
			Gender = patient.Gender,
			BirthDate = patient.BirthDate,
			Active = patient.Active,
		};

		var result = await _patientDbContext.Patients.AddAsync(model);

		return result.Entity.Id;
	}

	public async Task DeletePatientAsync(Guid id)
	{
		var patient = await _patientDbContext.Patients.FindAsync(id);
		_patientDbContext.Patients.Remove(patient!);
	}

	public async Task<Patient?> GetPatientAsync(Guid id)
	{
		var model = await _patientDbContext.Patients
			.FindAsync(id);

		if (model != null)
		{
			return new Patient
			{
				Id = model.Id,
				Name = new()
				{
					Family = model.Name.Family,
					Given = model.Name.Given,
					Use = model.Name.Use,
				},
				BirthDate = model.BirthDate,
				Gender = model.Gender,
				Active = model.Active,
			};
		}

		return null;
	}

	public async IAsyncEnumerable<Patient> GetAllPatientsAsync()
	{
		var stream = _patientDbContext.Patients
		.AsNoTracking()
		.AsAsyncEnumerable();

		await foreach (var patient in stream)
		{
			yield return new Patient
			{
				Id = patient.Id,
				Name = new()
				{
					Family = patient.Name.Family,
					Given = patient.Name.Given,
					Use = patient.Name.Use,
				},
				BirthDate = patient.BirthDate,
				Gender = patient.Gender,
				Active = patient.Active,
			};
		}
	}

	public IQueryable<Patient> GetAllPatientsQueryAsync()
	{
		return _patientDbContext.Patients
			.AsNoTracking()
			.Select(patient => new Patient
			{
				Id = patient.Id,
				Name = new()
				{
					Family = patient.Name.Family,
					Given = patient.Name.Given,
					Use = patient.Name.Use,
				},
				BirthDate = patient.BirthDate,
				Gender = patient.Gender,
				Active = patient.Active,
			});
	}

	public async Task UdpatePatientAsync(Patient patient)
	{
		var model = await _patientDbContext.Patients
			.Include(p => p.Name)
			.FirstOrDefaultAsync(p => p.Id == patient.Id);

		if (model != null)
		{
			model.BirthDate = patient.BirthDate;
			model.Gender = patient.Gender;
			model.Active = patient.Active;

			model.Name.Family = patient.Name.Family;
			model.Name.Given = patient.Name.Given;
			model.Name.Use = patient.Name.Use;
		}
	}

	public async Task SaveChangesAsync()
	{
		await _patientDbContext.SaveChangesAsync();
	}
}
