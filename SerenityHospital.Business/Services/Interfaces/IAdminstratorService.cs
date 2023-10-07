using SerenityHospital.Business.Dtos.AdminstratorDtos;
using SerenityHospital.Business.Dtos.RoleDtos;
using SerenityHospital.Business.Dtos.TokenDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IAdminstratorService
{
    Task CreateAsync(CreateAdminstratorDto dto);
    Task UpdateAsync(AdminstratorUpdateDto dto);
    Task UpdateByAdminAsync(string id,AdminstratorUpdateByAdminDto dto);
    Task<TokenResponseDto> LoginAsync(LoginAdminstratorDto dto);
    Task<TokenResponseDto> LoginWithRefreshTokenAsync(string refreshToken);
    Task<ICollection<AdminstratorListItemDto>> GetAllAsync(bool takeAll);
    Task SoftDeleteAsync(string id);
    Task RevertSoftDeleteAsync(string id);
    Task AddRoleAsync(AddRoleDto dto);
    Task RemoveRoleAsync(RemoveRoleDto dto);
    Task DeleteAsync(string id);
}

