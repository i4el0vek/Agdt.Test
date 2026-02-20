using Agdt.Core.Models;
using Agdt.Host.Commands.Patients;
using Agdt.Host.Commands.Patients.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Agdt.Host.Controllers
{
	/// <summary>
	/// Patient controller
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	public class PatientController : ControllerBase
	{
		private readonly IMediator _mediator;

		public PatientController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Get patient by id
		/// </summary>
		/// <param name="id">Patient Id</param>
		/// <returns>Patient</returns>
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id)
		{
			var result = await _mediator.Send(new GetPatientByIdQuery(id));
			if (result != null)
			{
				return Ok(result);
			}
			return NotFound();
		}

		/// <summary>
		/// Get patients and get by birthdate in format
		/// https://www.hl7.org/fhir/search.html#date
		/// </summary>
		/// <returns>List of patients</returns>
		[HttpGet]
		public IAsyncEnumerable<Patient> GetAll([FromQuery(Name = "date")] string[]? filter)
		{
			return _mediator.CreateStream(new GetAllPatientByBirthDateQuery(filter));
		}

		/// <summary>
		/// Create new patient
		/// </summary>
		/// <param name="patient">Patient model</param>
		/// <returns>Id</returns>
		[HttpPost]
		public async Task<IActionResult> Register([FromBody]CreatePatientCommand patient)
		{
			var result = await _mediator.Send(patient);
			return Ok(result.Id);
		}

		/// <summary>
		/// Edit patient
		/// </summary>
		/// <param name="patient">Patient model</param>
		[HttpPut]
		public async Task<IActionResult> Edit([FromBody] EditPatientCommand patient)
		{
			var result = await _mediator.Send(patient);
			if (result)
			{
				return Ok();
			}
			return NotFound();
		}

		/// <summary>
		/// Delete patient
		/// </summary>
		/// <param name="id">Id</param>
		[HttpDelete("{id}")]
		public async Task<IActionResult> Unregister(Guid id)
		{
			var result = await _mediator.Send(new DeletePatientByIdCommand(id));
			if (result)
			{
				return NoContent();
			}
			return NotFound();
		}
	}
}
