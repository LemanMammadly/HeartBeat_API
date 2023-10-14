using SerenityHospital.Core.Entities;
using SerenityHospital.DAL.Contexts;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.DAL.Repositories.Implements;

public class AppoinmentRepository : Repository<Appoinment>, IAppoinmentRepository
{
    public AppoinmentRepository(AppDbContext context) : base(context)
    {
    }
}

