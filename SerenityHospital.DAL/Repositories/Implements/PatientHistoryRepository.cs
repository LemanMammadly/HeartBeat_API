using SerenityHospital.Core.Entities;
using SerenityHospital.DAL.Contexts;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.DAL.Repositories.Implements;

public class PatientHistoryRepository : Repository<PatientHistory>, IPatientHistoryRepository
{
    public PatientHistoryRepository(AppDbContext context) : base(context)
    {
    }
}

