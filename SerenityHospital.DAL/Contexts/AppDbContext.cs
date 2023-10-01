using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.DAL.Contexts;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Hospital> Hospitals { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}




