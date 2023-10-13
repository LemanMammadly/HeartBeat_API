using SerenityHospital.Business.Dtos.DoctorRoom;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IDoctorRoomService
{
    Task<ICollection<DoctorRoomListItemDto>> GetAllAsync(bool takeAll);
    Task<DoctorRoomDetailItemDto> GetByIdAsync(int id, bool takeAll);
    Task CreateAsync(DoctorRoomCreateDro dto);
    Task UpdateAsync(int id, DoctorRoomUpdateDto dto);
    Task SoftDeleteAsync(int id);
    Task RevertSoftDeleteAsync(int id);
    Task DeleteAsync(int id);
}

