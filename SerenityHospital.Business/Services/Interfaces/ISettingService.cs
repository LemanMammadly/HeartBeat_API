using SerenityHospital.Business.Dtos.SettingDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface ISettingService
{
    Task<IEnumerable<SettingDetailItemDto>> GetAllAsync();
    Task CreateAsync(SettingCreateDto dto);
    Task UpdateAsync(int id, SettingUpdateDto dto);
}

