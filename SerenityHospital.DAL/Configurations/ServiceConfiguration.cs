using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.DAL.Configurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.Property(s => s.Name)
            .IsRequired();
        builder.Property(s => s.Description)
            .IsRequired();
        builder.Property(s => s.ServiceBeginning)
            .IsRequired();
        builder.Property(s => s.ServiceEnding)
            .IsRequired();
        builder.Property(s => s.MinPrice)
            .IsRequired()
            .HasDefaultValue(0);
        builder.Property(s => s.MaxPrice)
            .IsRequired()
            .HasDefaultValue(0);

    }
}

