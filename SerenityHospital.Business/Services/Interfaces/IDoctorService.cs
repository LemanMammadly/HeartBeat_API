using SerenityHospital.Business.Dtos.DoctorDtos;
using SerenityHospital.Business.Dtos.RoleDtos;
using SerenityHospital.Business.Dtos.TokenDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IDoctorService
{
    Task CreateAsync(DoctorCreateDto dto);
    Task<TokenResponseDto> LoginAsync(DoctorLoginDto dto);
    Task<TokenResponseDto> LoginWithRefreshTokenAsync(string refreshToken);
    Task<ICollection<DoctorListItemDto>> GetAllAsync(bool takeAll);
    Task<DoctorDetailItemDto> GetById(string id, bool takeAll);
    Task<DoctorDetailItemDto> GetByUsername(string userName, bool takeAll);
    Task AddDoctorRoom(AddDoctorRoomDto dto);
    Task<int> Count();
    Task UpdateAsync(DoctorUpdateDto dto);
    Task UpdateByAdminAsync(string id,DoctorUpdateByAdminDto dto);
    Task AddRole(AddRoleDto dto);
    Task RemoveRole(RemoveRoleDto dto);
    Task SoftDeleteAsync(string id);
    Task ReverteSoftDeleteAsync(string id);
    Task DeleteAsync(string id);
    Task Logout();
    Task DoctorStatusUpdater(string id);
}


