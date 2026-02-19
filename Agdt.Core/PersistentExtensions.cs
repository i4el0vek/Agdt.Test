using Agdt.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Agdt.Core;

public static class CoreExtensions
{
	public static IServiceCollection AddCore(this IServiceCollection services)
	{
		services.AddScoped<PatientService>();
		return services;
	}
}
