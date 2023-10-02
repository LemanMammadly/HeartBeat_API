using SerenityHospital.Core.Entities;
using SerenityHospital.DAL.Contexts;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.DAL.Repositories.Implements;

public class PatientRoomRepository : Repository<PatientRoom>, IPatientRoomRepository
{
    public PatientRoomRepository(AppDbContext context) : base(context)
    {
    }
}

