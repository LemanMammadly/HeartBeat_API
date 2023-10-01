using SerenityHospital.Core.Entities;
using SerenityHospital.DAL.Contexts;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.DAL.Repositories.Implements;

public class ServiceRepository : Repository<Service>, IServiceRepository
{
    public ServiceRepository(AppDbContext context) : base(context)
    {
    }
}

