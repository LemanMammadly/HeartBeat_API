using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.DAL.Configurations;

public class PatientHistoryConfiguration : IEntityTypeConfiguration<PatientHistory>
{
    public void Configure(EntityTypeBuilder<PatientHistory> builder)
    {
        builder.HasOne(ph => ph.Recipe)
            .WithOne(r => r.PatientHistory)
            .HasForeignKey<PatientHistory>(ph => ph.RecipeId);
        builder.HasOne(ph => ph.Patient)
            .WithMany(p => p.PatientHistories)
            .HasForeignKey(ph => ph.PatientId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(ph => ph.Doctor)
            .WithMany(d => d.PatientHistories)
            .HasForeignKey(ph => ph.DoctorId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Property(ph => ph.Date)
            .IsRequired();
    }
}

