using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.DAL.Configurations;

public class AppoinmentConfiguration : IEntityTypeConfiguration<Appoinment>
{
    public void Configure(EntityTypeBuilder<Appoinment> builder)
    {
        builder.HasOne(a => a.Doctor)
            .WithMany(d => d.Appoinments)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(a => a.Patient)
            .WithMany(p => p.Appoinments)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(a => a.AppoinmentAsDoctor)
            .WithMany(d => d.AppointmentsAsPatient)
            .HasForeignKey(a => a.AppoinmentAsDoctorId);
        builder.Property(a => a.ProblemDesc)
            .IsRequired();
        builder.Property(a => a.AppoinmentDate)
            .IsRequired();
        builder.Property(a => a.Duration)
            .HasDefaultValue(20)
            .IsRequired();
        builder.Property(a => a.Status)
            .IsRequired();
        builder.Property(a => a.AppoinmentMoney)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(20.0m)
            .IsRequired();
    }
}

