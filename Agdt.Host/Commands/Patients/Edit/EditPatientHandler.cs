using Agdt.Core.Models;
using Agdt.Core.Services;
using MediatR;

namespace Agdt.Host.Commands.Patients.Create;

public class EditPatientHandler : IRequestHandler<EditPatientCommand, bool>
{
	private readonly PatientService _patientService;

	public EditPatientHandler(PatientService patientService)
	{
		_patientService = patientService;
	}

	public async Task<bool> Handle(EditPatientCommand request, CancellationToken cancellationToken)
	{
		var patient = new Patient()
		{
			Id = request.Id,
			Active = request.Active,
			Name = new()
			{
				Family = request.Name.Family,
				Use = request.Name.Use,
				Given = request.Name.Given,
			},
			BirthDate = request.BirthDate!.Value,
			Gender = request.Gender
		};

		return await _patientService.UdpateAsync(patient);
	}
}
