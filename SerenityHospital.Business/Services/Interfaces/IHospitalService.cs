using SerenityHospital.Business.Dtos.HospitalDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IHospitalService
{
    Task<IEnumerable<HospitalDetailItemDto>> GetAllAsync();
    Task CreateAsync(HospitalCreateDto dto);
    Task UpdateAsync(int id, HospitalUpdateDto dto);
}

