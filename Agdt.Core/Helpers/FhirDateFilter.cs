namespace Agdt.Core.Helpers;

public record FhirDateFilter(string Prefix, DateTimeOffset Date)
{
	public static FhirDateFilter Parse(string input)
	{
		string[] prefixes = { "eq", "ne", "gt", "lt", "ge", "le", "sa", "eb", "ap" };
		string prefix = "eq";
		string datePart = input;

		if (input.Length > 2 && prefixes.Contains(input[..2]))
		{
			prefix = input.Substring(0, 2);
			datePart = input.Substring(2);
		}

		if (DateTimeOffset.TryParse(datePart, out var date))
		{
			return new FhirDateFilter(prefix, date);
		}

		throw new ArgumentException("Некорректный формат даты");
	}
}
