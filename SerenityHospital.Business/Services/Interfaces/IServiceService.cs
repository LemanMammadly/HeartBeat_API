using SerenityHospital.Business.Dtos.ServiceDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IServiceService
{
    Task<IEnumerable<ServiceListItemDto>> GetAllAsync(bool takeAll);
    Task<ServiceDetailItemDto> GetByIdAsync(int id,bool takeAll);
    Task CreateAsync(ServiceCreateDto dto);
    Task UpdateAsync(int id,ServiceUpdateDto dto);
    Task DeleteAsync(int id);
    Task SoftDeleteAsync(int id);
    Task ReverteSoftDeleteAsync(int id);
}

