using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.DAL.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.Property(p => p.BloodType)
            .IsRequired();
        builder.Property(p => p.Address)
            .IsRequired();
        builder.HasOne(p => p.PatientRoom)
            .WithMany(pr => pr.Patients)
            .HasForeignKey(p => p.PatientRoomId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);
    }
}

