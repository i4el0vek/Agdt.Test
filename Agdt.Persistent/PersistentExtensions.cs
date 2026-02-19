using Agdt.Core.Interfaces;
using Agdt.Persistent.Repositories;
using Agdt.Persistent.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Agdt.Persistent;

public static class PersistentExtensions
{
	public static async Task InitDbAsync(this IServiceProvider services)
	{
		using var scope = services.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<PatientDbContext>();

		await dbContext.Database.EnsureDeletedAsync();
		await dbContext.Database.EnsureCreatedAsync();
	}

	public static IServiceCollection AddPersistent(this IServiceCollection services, string connectionString)
	{
		services.AddDbContext<PatientDbContext>(options => options.UseNpgsql(connectionString));
		services.AddScoped<IPatientRepository, PatientRepository>();
		return services;
	}
}
