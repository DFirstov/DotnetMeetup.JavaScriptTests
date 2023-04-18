using Microsoft.EntityFrameworkCore;
using SampleWebAPI.Models;

namespace SampleWebAPI.Data;

public class FallingTimeContext : DbContext
{
	public FallingTimeContext(DbContextOptions<FallingTimeContext> options) : base(options)
	{
	}
	
	public DbSet<GravityAcceleration> GravityAccelerations { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<GravityAcceleration>().ToTable("GravityAcceleration");
	}
}