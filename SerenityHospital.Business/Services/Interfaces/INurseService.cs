using SerenityHospital.Business.Dtos.NurseDtos;
using SerenityHospital.Business.Dtos.RoleDtos;
using SerenityHospital.Business.Dtos.TokenDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface INurseService
{
    Task CreateAsync(NurseCreateDto dto);
    Task<TokenResponseDto> LoginAsync(NurseLoginDto dto);
    Task<TokenResponseDto> LoginWithRefreshTokenAsync(string refreshToken);
    Task<ICollection<NurseListItemDto>> GetAllAsync(bool takeAll);
    Task<NurseDetailItemDto> GetById(bool takeAll,string id);
    Task<NurseDetailItemDto> GetByName(string username);
    Task UpdateAsync(NurseUpdateDto dto);
    Task<int> Count();
    Task UpdateByAdminAsync(string id,NurseUpdateByAdminDto dto);
    Task AddRole(AddRoleDto dto);
    Task RemoveRole(RemoveRoleDto dto);
    Task SoftDeleteAsync(string id);
    Task RevertSoftDeleteAsync(string id);
    Task DeleteAsync(string id);
    Task Logout();
}

