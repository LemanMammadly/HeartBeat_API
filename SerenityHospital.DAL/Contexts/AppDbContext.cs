using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.DAL.Contexts;

public class AppDbContext:IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Hospital> Hospitals { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<PatientRoom> PatientRooms { get; set; }
    public DbSet<Adminstrator> Adminstrators { get; set; }
    public DbSet<Nurse> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Nurse> Nurses { get; set; }
    public DbSet<DoctorRoom> DoctorRooms { get; set; }
    public DbSet<Appoinment> Appoinments { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<PatientHistory> PatientHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}









