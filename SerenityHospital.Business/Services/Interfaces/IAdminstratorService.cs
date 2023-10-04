using SerenityHospital.Business.Dtos.AdminstratorDtos;
using SerenityHospital.Business.Dtos.TokenDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IAdminstratorService
{
    Task CreateAsync(CreateAdminstratorDto dto);
    Task<TokenResponseDto> LoginAsync(LoginAdminstratorDto dto);
    Task SoftDeleteAsync(string id);
    Task RevertSoftDeleteAsync(string id);
}

