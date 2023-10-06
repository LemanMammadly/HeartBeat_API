using SerenityHospital.Business.Dtos.DoctorDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IDoctorService
{
    Task CreateAsync(DoctorCreateDto dto);
}

