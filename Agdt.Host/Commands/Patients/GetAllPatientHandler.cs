using Agdt.Core.Models;
using Agdt.Core.Services;
using MediatR;

namespace Agdt.Host.Commands.Patients;

public record GetAllPatientQuery() : IStreamRequest<Patient>;

public class GetAllPatientHandler : IStreamRequestHandler<GetAllPatientQuery, Patient>
{
	private readonly PatientService _patientService;
	public GetAllPatientHandler(PatientService patientService)
	{
		_patientService = patientService;
	}

	public IAsyncEnumerable<Patient> Handle(GetAllPatientQuery request, CancellationToken cancellationToken)
	{
		var patients = _patientService.GetAllPatients();
		return patients;
	}
}
