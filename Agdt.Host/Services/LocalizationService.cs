using Agdt.Host.Localization;

namespace Agdt.Host.Services;

public class LocalizationService
{
	public string GetLocalizedException(string key, params object[] args)
	{
		if (key == ErrorKeys.PatientNotFound)
		{
			return $"Patient with {args[0]} not found";
		}

		return "-";
	}
}
