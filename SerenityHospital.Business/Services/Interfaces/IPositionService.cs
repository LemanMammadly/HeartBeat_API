using SerenityHospital.Business.Dtos.PositionDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IPositionService
{
    Task<IEnumerable<PositionListItemDto>> GetAllAsync(bool takeAll);
    Task<PositionDetailItemDto> GetByIdAsync(int id, bool takeAll);
    Task CreateAsync(PositionCreateDto dto);
    Task UpdateAsync(int id, PositionUpdateDto dto);
    Task DeleteAsync(int id);
    Task<int> Count();
    Task SoftDeleteAsync(int id);
    Task ReverteSoftDeleteAsync(int id);
}

