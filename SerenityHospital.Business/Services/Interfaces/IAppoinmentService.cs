using SerenityHospital.Business.Dtos.AppoinmentDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IAppoinmentService
{
    Task<ICollection<AppoinmentListItemDto>> GetAllAsync(bool takeAll);
    Task<AppoinmentDetailItemDto> GetByIdAsync(int id, bool takeAll);
    Task CreateAsync(AppoinmentCreateDto dto);
    Task UpdateAsync(int id,AppoinmentUpdateDto dto);
}

