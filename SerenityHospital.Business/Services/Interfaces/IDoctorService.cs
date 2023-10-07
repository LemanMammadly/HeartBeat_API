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
    Task UpdateAsync(DoctorUpdateDto dto);
    Task UpdateByAdminAsync(string id,DoctorUpdateByAdminDto dto);
    Task AddRole(AddRoleDto dto);
    Task RemoveRole(RemoveRoleDto dto);
    Task SoftDeleteAsync(string id);
    Task ReverteSoftDeleteAsync(string id);
}


