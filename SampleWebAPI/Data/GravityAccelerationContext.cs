using Microsoft.EntityFrameworkCore;
using SampleWebAPI.Models;

namespace SampleWebAPI.Data;

public class GravityAccelerationContext : DbContext
{
	public GravityAccelerationContext(DbContextOptions<GravityAccelerationContext> options) : base(options)
	{
	}

	public DbSet<GravityAcceleration> GravityAccelerations { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder
			.Entity<GravityAcceleration>()
			.ToTable("GravityAcceleration")
			.HasKey(ga => ga.Name);
	}
}
