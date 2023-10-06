using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.DAL.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.Property(d => d.Description)
            .IsRequired();
        builder.Property(d => d.Salary)
            .IsRequired();
        builder.Property(d => d.StartDate)
            .HasDefaultValueSql("DATEADD(hour, 4, GETUTCDATE())")
            .IsRequired();
        builder.Property(d => d.EndDate)
            .IsRequired(false);
        builder.Property(d => d.Status)
            .IsRequired();
        builder.Property(d => d.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();
        builder.HasOne(d => d.Department)
            .WithMany(d => d.Doctors)
            .HasForeignKey(d => d.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(d => d.Position)
            .WithMany(p => p.Doctors)
            .HasForeignKey(d => d.PositionId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

