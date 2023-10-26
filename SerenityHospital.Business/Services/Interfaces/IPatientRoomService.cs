using SerenityHospital.Business.Dtos.PatientRoomDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IPatientRoomService
{
    Task<IEnumerable<PatientRoomListItemDto>> GetAllAsync(bool takeAll);
    Task<PatientRoomDetailItemDto> GetByIdAsync(int id, bool takeAll);
    Task CreateAsync(PatientRoomCreateDto dto);
    Task UpdateAsync(int id, PatientRoomUpdateDto dto);
    Task DeleteAsync(int id);
    Task<int> Count();
    Task SoftDeleteAsync(int id);
    Task RevertSoftDeleteAsync(int id);
}

