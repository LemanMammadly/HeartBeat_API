using SerenityHospital.Business.Dtos.PatientDtos;
using SerenityHospital.Business.Dtos.RoleDtos;
using SerenityHospital.Business.Dtos.TokenDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IPatientService
{
    Task CreateAsync(PatientCreateDto dto);
    Task<TokenResponseDto> LoginAsync(PatientLoginDto dto);
    Task<TokenResponseDto> LoginWithRefreshTokenAsync(string refreshToken);
    Task AddRole(AddRoleDto dto);
    Task<ICollection<PatientListItemDto>> GetAllAsync(bool takeAll);
}

