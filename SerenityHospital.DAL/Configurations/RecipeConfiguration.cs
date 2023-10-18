using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.DAL.Configurations;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.Property(r => r.RecipeDesc)
            .IsRequired();
        builder.HasOne(r => r.Appoinment)
            .WithOne(a => a.Recipe)
            .HasForeignKey<Recipe>(r => r.AppoinmentId);
        builder.HasOne(r => r.Doctor)
            .WithMany(d => d.Recipes)
            .HasForeignKey(r => r.DoctorId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(r => r.Patient)
            .WithMany(p => p.Recipes)
            .HasForeignKey(r => r.PatientId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

