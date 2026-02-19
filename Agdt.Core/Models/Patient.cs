using Agdt.Core.Enums;

namespace Agdt.Core.Models;

public class Patient
{
	public Guid Id { get; set; }
	public Name Name { get; set; }
	public Gender Gender { get; set; }
	public DateTimeOffset BirthDate { get; set; }
	public bool Active { get; set; }
}

