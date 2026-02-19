using Agdt.Persistent.Entities;
using Microsoft.EntityFrameworkCore;

namespace Agdt.Persistent.Services;

public class PatientDbContext : DbContext
{
	public DbSet<PatientEntity> Patients => Set<PatientEntity>();

	public PatientDbContext(DbContextOptions<PatientDbContext> options)
		: base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<PatientEntity>(entity =>
		{
			entity.HasKey(e => e.Id);

			entity.HasIndex(e => e.BirthDate);

			entity.OwnsOne(e => e.Name, name =>
			{
				name.Property(n => n.Given)
					.HasColumnType("jsonb");
			});

			entity.Property(e => e.Gender).IsRequired();
		});
	}
}
