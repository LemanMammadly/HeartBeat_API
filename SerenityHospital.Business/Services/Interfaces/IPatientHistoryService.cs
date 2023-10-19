using SerenityHospital.Business.Dtos.PatientDtos;
using SerenityHospital.Business.Dtos.PatientHistoryDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IPatientHistoryService
{
    Task<ICollection<PatientHistoryListItemDto>> GetAllAsync();
    Task<PatientHistoryDetailtemDto> GetByIdAsync(int id);
    Task CreateAsync(PatientHistory patientHistory);
    Task UpdateAsync(PatientHistory patientHistory);
}

