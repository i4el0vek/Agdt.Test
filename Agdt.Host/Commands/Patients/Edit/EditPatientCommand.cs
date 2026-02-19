using Agdt.Core.Enums;
using Agdt.Core.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Agdt.Host.Commands.Patients.Create;

public class EditPatientNameCommand
{
	public string Use { get; set; }

	[Required]
	public string Family { get; set; }

	public string[] Given { get; set; }
}

public class EditPatientCommand : IRequest<bool>
{
	[Required]
	public Guid Id { get; set; }
	public EditPatientNameCommand Name { get; set; }

	[Required]
	public DateTimeOffset? BirthDate { get; set; }

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public Gender Gender { get; set; }

	public UserStatus Active { get; set; }
}
