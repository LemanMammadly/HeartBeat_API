using SerenityHospital.Business.Dtos.DoctorDtos;
using SerenityHospital.Business.Dtos.RoleDtos;
using SerenityHospital.Business.Dtos.TokenDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IDoctorService
{
    Task CreateAsync(DoctorCreateDto dto);
    Task<TokenResponseDto> LoginAsync(DoctorLoginDto dto);
    Task<ICollection<DoctorListItemDto>> GetAllAsync(bool takeAll);
    Task AddRole(AddRoleDto dto);
}

