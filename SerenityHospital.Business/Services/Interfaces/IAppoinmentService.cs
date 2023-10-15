using SerenityHospital.Business.Dtos.AppoinmentDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IAppoinmentService
{
    Task<ICollection<AppoinmentListItemDto>> GetAllAsync(bool takeAll);
    Task CreateAsync(AppoinmentCreateDto dto);
}

