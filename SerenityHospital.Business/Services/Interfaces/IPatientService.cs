using SerenityHospital.Business.Dtos.PatientDtos;
using SerenityHospital.Business.Dtos.RoleDtos;
using SerenityHospital.Business.Dtos.TokenDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IPatientService
{
    Task CreateAsync(PatientCreateDto dto);
    Task AddPatientRoom(AddPatientRoomDto dto);
    Task<TokenResponseDto> LoginAsync(PatientLoginDto dto);
    Task<TokenResponseDto> LoginWithRefreshTokenAsync(string refreshToken);
    Task<ICollection<PatientListItemDto>> GetAllAsync();
    Task AddRole(AddRoleDto dto);
    Task RemoveRole(RemoveRoleDto dto);
    Task UpdateAsync(PatientUpdateDto dto);
    Task UpdateByAdminAsync(string id,PatientUpdateByAdminDto dto);
    Task DeleteAsync(string id);
    Task Logout();
}

