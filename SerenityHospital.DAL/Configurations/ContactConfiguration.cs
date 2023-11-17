using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.DAL.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.Property(c => c.Name)
            .HasMaxLength(25)
            .IsRequired();
        builder.Property(c => c.Email)
            .IsRequired();
        builder.Property(c => c.Phone)
            .IsRequired();
        builder.Property(c => c.Address)
            .IsRequired();
        builder.Property(c => c.Message)
            .IsRequired();
        builder.Property(c=>c.Date)
            .HasDefaultValueSql("DATEADD(hour, 4, GETUTCDATE())");
        builder.Property(c => c.IsRead)
            .HasDefaultValue(false);
    }
}

