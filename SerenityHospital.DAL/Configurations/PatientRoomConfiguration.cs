using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.DAL.Configurations;

public class PatientRoomConfiguration : IEntityTypeConfiguration<PatientRoom>
{
    public void Configure(EntityTypeBuilder<PatientRoom> builder)
    {
        builder.Property(pr => pr.Number)
             .IsRequired();
        builder.Property(pr => pr.Type)
            .IsRequired();
        builder.Property(pr => pr.Status)
            .IsRequired();
        builder.Property(pr => pr.Capacity)
            .IsRequired();
        builder.Property(pr => pr.Price)
            .IsRequired();
        builder.Property(pr => pr.ImageUrl)
            .IsRequired();
        builder.HasOne(pr => pr.Department)
            .WithMany(d => d.PatientRooms)
            .HasForeignKey(pr => pr.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}

