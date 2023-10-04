using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.DAL.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(au => au.Name)
             .HasMaxLength(25)
             .IsRequired();
        builder.Property(au => au.Surname)
            .HasMaxLength(25)
            .IsRequired();
        builder.Property(au => au.Age)
            .IsRequired();
        builder.Property(au => au.Gender)
            .IsRequired();
        builder.Property(au => au.ImageUrl)
            .IsRequired(false);
    }
}

