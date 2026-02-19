using Agdt.Core.Enums;

namespace Agdt.Persistent.Entities;

public record PatientEntity
{
	public Guid Id { get; set; }
	public NameEntity Name { get; set; }
	public Gender Gender { get; set; }
	public DateTimeOffset BirthDate { get; set; }
	public bool Active { get; set; } 
}

