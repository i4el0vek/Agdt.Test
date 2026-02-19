using System.Text.Json;
using System.Text.Json.Serialization;

namespace Agdt.Host.Helpers;

public static class JsonHelper
{
	private static JsonSerializerOptions GetOptions()
	{
		var options = new JsonSerializerOptions();
		options.Converters.Add(new JsonStringEnumConverter());

		options.PropertyNameCaseInsensitive = true;
		options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

		return options;
	}

	public static readonly JsonSerializerOptions DefaultJsonOptions = GetOptions();

	public static JsonSerializerOptions MapOptions(this JsonSerializerOptions options, JsonSerializerOptions newOptions)
	{
		foreach (var item in newOptions.Converters)
		{
			options.Converters.Add(item);
		}

		options.PropertyNameCaseInsensitive = newOptions.PropertyNameCaseInsensitive;
		options.PropertyNamingPolicy = newOptions.PropertyNamingPolicy;

		return options;
	}

	public static string ToJson(this object obj)
	{
		return JsonSerializer.Serialize(obj, DefaultJsonOptions);
	}
}
