using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.DAL.Configurations;

public class HospitalConfiguration : IEntityTypeConfiguration<Hospital>
{
    public void Configure(EntityTypeBuilder<Hospital> builder)
    {
        builder.Property(h => h.Name)
            .IsRequired()
            .HasMaxLength(64);
        builder.Property(h => h.Description)
            .IsRequired();
        builder.Ignore(s => s.IsDeleted);
    }
}

