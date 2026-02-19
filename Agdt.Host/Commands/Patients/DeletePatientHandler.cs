using Agdt.Core.Services;
using MediatR;

namespace Agdt.Host.Commands.Patients;

public record DeletePatientByIdCommand(Guid Id) : IRequest<bool>;

public class DeletePatientHandler : IRequestHandler<DeletePatientByIdCommand, bool>
{
	private readonly PatientService _patientService;
	public DeletePatientHandler(PatientService patientService)
	{
		_patientService = patientService;
	}

	public async Task<bool> Handle(DeletePatientByIdCommand request, CancellationToken cancellationToken)
	{
		return await _patientService.DeregisterAsync(request.Id);
	}
}
