using Microsoft.EntityFrameworkCore;
using WellBeingSense.Api.Models;

namespace WellBeingSense.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<WellBeingCheckin> WellBeingCheckins => Set<WellBeingCheckin>();
    public DbSet<EnvironmentReading> EnvironmentReadings => Set<EnvironmentReading>();
    public DbSet<RiskAlert> RiskAlerts => Set<RiskAlert>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>()
            .HasMany(e => e.Checkins)
            .WithOne(c => c.Employee!)
            .HasForeignKey(c => c.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, Name = "Ana Silva", Department = "TI", Role = "Dev", IsActive = true },
            new Employee { Id = 2, Name = "Bruno Souza", Department = "RH", Role = "Analista", IsActive = true }
        );
    }
}
