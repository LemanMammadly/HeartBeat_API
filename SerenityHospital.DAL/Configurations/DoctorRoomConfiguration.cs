using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.DAL.Configurations;

public class DoctorRoomConfiguration : IEntityTypeConfiguration<DoctorRoom>
{
    public void Configure(EntityTypeBuilder<DoctorRoom> builder)
    {
        builder.HasIndex(dr=>dr.Number)
               .IsUnique();
        builder.HasOne(dr => dr.Department)
            .WithMany(d => d.DoctorRooms)
            .HasForeignKey(dr => dr.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

