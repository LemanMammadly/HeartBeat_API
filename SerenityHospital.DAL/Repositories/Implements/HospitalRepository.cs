using SerenityHospital.Core.Entities;
using SerenityHospital.DAL.Contexts;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.DAL.Repositories.Implements;

public class HospitalRepository : Repository<Hospital>, IHospitalRepository
{
    public HospitalRepository(AppDbContext context) : base(context)
    {
    }
}

