using SerenityHospital.Core.Entities;
using SerenityHospital.DAL.Contexts;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.DAL.Repositories.Implements;

public class DoctorRoomRepository : Repository<DoctorRoom>, IDoctorRoomRepository
{
    public DoctorRoomRepository(AppDbContext context) : base(context)
    {
    }
}

