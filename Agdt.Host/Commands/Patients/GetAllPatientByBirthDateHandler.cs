using Agdt.Core.Models;
using Agdt.Core.Services;
using MediatR;

namespace Agdt.Host.Commands.Patients;

public record GetAllPatientByBirthDateQuery(string[]? Filter) : IStreamRequest<Patient>;

public class GetAllPatientByBirthDateHandler : IStreamRequestHandler<GetAllPatientByBirthDateQuery, Patient>
{
	private readonly PatientService _patientService;
	public GetAllPatientByBirthDateHandler(PatientService patientService)
	{
		_patientService = patientService;
	}

	public IAsyncEnumerable<Patient> Handle(GetAllPatientByBirthDateQuery request, CancellationToken cancellationToken)
	{
		var patients = _patientService.GetAllPatientsByBirthDateAsync(request.Filter);
		return patients;
	}
}
