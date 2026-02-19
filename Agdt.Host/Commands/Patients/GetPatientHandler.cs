using Agdt.Core.Models;
using Agdt.Core.Services;
using MediatR;

namespace Agdt.Host.Commands.Patients;

public record GetPatientByIdQuery(Guid Id) : IRequest<Patient>;

public class GetPatientHandler : IRequestHandler<GetPatientByIdQuery, Patient>
{
	private readonly PatientService _patientService;
	public GetPatientHandler(PatientService patientService)
	{
		_patientService = patientService;
	}

	public Task<Patient> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
	{
		var patient = _patientService.GetPatientAsync(request.Id);
		return patient;
	}
}
