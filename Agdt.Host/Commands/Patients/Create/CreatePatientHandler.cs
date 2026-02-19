using Agdt.Core.Models;
using Agdt.Core.Services;
using MediatR;

namespace Agdt.Host.Commands.Patients.Create;

public class CreatePatientHandler : IRequestHandler<CreatePatientCommand, Patient>
{
	private readonly PatientService _patientService;

	public CreatePatientHandler(PatientService patientService)
	{
		_patientService = patientService;
	}

	public async Task<Patient> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
	{
		var patient = new Patient()
		{
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
		patient.Id = await _patientService.RegisterAsync(patient);
		return patient;
	}
}
