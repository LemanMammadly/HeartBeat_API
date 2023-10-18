using SerenityHospital.Business.Dtos.PatientDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IPatientHistoryService
{
    Task<ICollection<PatientListItemDto>> GetAllAsync();
    Task<PatientDetailItemDto> GetByIdAsync(int id);
    Task CreateAsync(PatientHistory patientHistory);
}

