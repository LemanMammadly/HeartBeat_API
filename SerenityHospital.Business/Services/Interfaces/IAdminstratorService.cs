using SerenityHospital.Business.Dtos.AdminstratorDtos;
using SerenityHospital.Business.Dtos.RoleDtos;
using SerenityHospital.Business.Dtos.TokenDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IAdminstratorService
{
    Task CreateAsync(CreateAdminstratorDto dto);
    Task<TokenResponseDto> LoginAsync(LoginAdminstratorDto dto);
    Task<ICollection<AdminstratorListItemDto>> GetAllAsync();
    Task SoftDeleteAsync(string id);
    Task RevertSoftDeleteAsync(string id);
    Task AddRoleAsync(AddRoleDto dto);
    Task RemoveRoleAsync(RemoveRoleDto dto);
}

