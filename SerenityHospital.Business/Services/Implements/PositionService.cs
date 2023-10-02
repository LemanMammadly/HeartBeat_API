using SerenityHospital.Business.Dtos.PositionDtos;
using SerenityHospital.Business.Services.Interfaces;

namespace SerenityHospital.Business.Services.Implements;

public class PositionService : IPositionService
{
    public Task CreateAsync(PositionCreateDto dto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PositionListItemDto>> GetAllAsync(bool takeAll)
    {
        throw new NotImplementedException();
    }

    public Task<PositionDetailItemDto> GetByIdAsync(int id, bool takeAll)
    {
        throw new NotImplementedException();
    }

    public Task ReverteSoftDeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task SoftDeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(int id, PositionUpdateDto dto)
    {
        throw new NotImplementedException();
    }
}

