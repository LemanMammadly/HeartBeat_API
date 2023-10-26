using SerenityHospital.Business.Dtos.DepartmentDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentListItemDto>> GetAllAsync(bool takeAll);
    Task<DepartmentDetailItemDto> GetByIdAsync(int id, bool takeAll);
    Task CreateAsync(DepartmentCreateDto dto);
    Task UpdateAsync(int id, DepartmentUpdateDto dto);
    Task DeleteAsync(int id);
    Task<int> Count();
    Task SoftDeleteAsync(int id);
    Task ReverteSoftDeleteAsync(int id);
}

