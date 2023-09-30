using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.DAL.Configurations;

public class SettingConfiguration : IEntityTypeConfiguration<Setting>
{
    public void Configure(EntityTypeBuilder<Setting> builder)
    {
        builder.Property(s => s.HeaderLogoUrl)
            .IsRequired();
        builder.Property(s => s.FooterLogoUrl)
            .IsRequired();
        builder.Property(s => s.Address)
            .IsRequired();
        builder.Property(s => s.Phone)
            .HasAnnotation("RegularExpression", @"^(\+994|0)(50|51|55|70|77|99)[1-9]\d{6}$")
            .IsRequired();
        builder.Property(s => s.Email)
            .HasAnnotation("RegularExpression", "[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}")
            .IsRequired();
        builder.Ignore(s => s.IsDeleted);
    }
}

