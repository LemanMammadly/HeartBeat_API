using SerenityHospital.Business.Dtos.AdminDtos;
using SerenityHospital.Business.Dtos.RoleDtos;
using SerenityHospital.Business.Dtos.TokenDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IAdminService
{
    Task CreateAsync(AdminCreateDto dto);
    Task UpdateAsync(AdminUpdateDto dto);
    Task UpdateByAdminAsync(string id,AdminUpdateBySuperAdminDto dto);
    Task<TokenResponseDto> LoginAsync(AdminLoginDto dto);
    Task<TokenResponseDto> LoginWithRefreshTokenAsync(string refreshToken);
    Task<ICollection<AdminListItemDto>> GetAllAsync();
    Task<AdminDetailItemDto> GetById(string id);
    Task AddRoleAsync(AddRoleDto dto);
    Task RemoveRoleAsync(RemoveRoleDto dto);
    Task DeleteAsync(string id);
    Task Logout();
}

