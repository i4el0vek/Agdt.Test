namespace Agdt.Persistent.Entities;

public record NameEntity
{
	public string Use { get; set; }
	public string Family { get; set; }
	public string[] Given { get; set; }
}

