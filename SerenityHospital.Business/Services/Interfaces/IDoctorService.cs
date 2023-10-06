using SerenityHospital.Business.Dtos.DoctorDtos;
using SerenityHospital.Business.Dtos.TokenDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IDoctorService
{
    Task CreateAsync(DoctorCreateDto dto);
    Task<TokenResponseDto> LoginAsync(DoctorLoginDto dto);
}

