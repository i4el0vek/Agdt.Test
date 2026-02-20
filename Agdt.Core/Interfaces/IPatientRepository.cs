using Agdt.Core.Models;

namespace Agdt.Core.Interfaces;

public interface IPatientRepository
{
	public Task<Patient?> GetPatientAsync(Guid id);
	public IAsyncEnumerable<Patient> GetAllPatientsAsync();
	public IQueryable<Patient> GetAllPatientsQueryAsync();
	public Task<Guid> AddPatientAsync(Patient patient);
	public Task UdpatePatientAsync(Patient patient);
	public Task DeletePatientAsync(Guid id);
	public Task SaveChangesAsync();
}
