using SerenityHospital.Business.Dtos.AppoinmentDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IAppoinmentService
{
    Task CreateAsync(AppoinmentCreateDto dto);
}

