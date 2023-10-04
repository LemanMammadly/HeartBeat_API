using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.DAL.Configurations;

public class AdminstratorConfiguration : IEntityTypeConfiguration<Adminstrator>
{
    public void Configure(EntityTypeBuilder<Adminstrator> builder)
    {
        builder.Property(a => a.Description)
             .IsRequired();
        builder.Property(a => a.Salary)
            .IsRequired();
        builder.Property(a => a.StartDate)
            .HasDefaultValueSql("DATEADD(hour, 4, GETUTCDATE())")
            .IsRequired();
        builder.Property(a => a.EndDate)
            .IsRequired(false);
        builder.Property(a => a.Status)
            .IsRequired();
        builder.HasOne(a => a.Hospital)
            .WithOne(h => h.Adminstrator)
            .HasForeignKey<Adminstrator>(a => a.HospitalId)
            .IsRequired(false);
    }
}

