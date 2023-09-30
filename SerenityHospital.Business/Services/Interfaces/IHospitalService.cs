using SerenityHospital.Business.Dtos.HospitalDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IHospitalService
{
    Task<IEnumerable<HospitalDetailItemDto>> GetAllAsync();
    Task UpdateAsync(int id, HospitalUpdateDto dto);
}

