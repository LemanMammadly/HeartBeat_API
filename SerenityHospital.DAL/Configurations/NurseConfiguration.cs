using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.DAL.Configurations;

public class NurseConfiguration : IEntityTypeConfiguration<Nurse>
{
    public void Configure(EntityTypeBuilder<Nurse> builder)
    {
        builder.Property(n => n.Description)
            .IsRequired();
        builder.Property(n => n.Salary)
            .IsRequired();
        builder.Property(n => n.StartDate)
            .HasDefaultValueSql("DATEADD(hour, 4, GETUTCDATE())")
            .IsRequired();
        builder.Property(n => n.EndDate)
            .IsRequired(false);
        builder.Property(n => n.Status)
            .IsRequired();
        builder.Property(n => n.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();
        builder.Property(n => n.StartWork)
            .IsRequired();
        builder.Property(n => n.EndWork)
            .IsRequired();
        builder.HasOne(n => n.Department)
            .WithMany(d => d.Nurses)
            .HasForeignKey(n => n.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

