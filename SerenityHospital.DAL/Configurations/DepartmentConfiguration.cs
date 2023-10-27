using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.DAL.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.Property(d => d.Name)
             .IsRequired();
        builder.Property(d => d.Description)
            .IsRequired();
        builder.Property(d => d.IconUrl)
            .IsRequired();
        builder.HasOne(b => b.Service)
            .WithMany(s => s.Departments)
            .HasForeignKey(b => b.ServiceId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

