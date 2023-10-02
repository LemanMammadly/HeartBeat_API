using SerenityHospital.Core.Entities;
using SerenityHospital.DAL.Contexts;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.DAL.Repositories.Implements;

public class PositionRepository : Repository<Position>, IPositionRepository
{
    public PositionRepository(AppDbContext context) : base(context)
    {
    }
}

