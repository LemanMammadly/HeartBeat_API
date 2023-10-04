using SerenityHospital.Business.Dtos.AdminstratorDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IAdminstratorService
{
    Task CreateAsync(CreateAdminstratorDto dto);
    Task SoftDeleteAsync(string id);
    Task RevertSoftDeleteAsync(string id);
}

