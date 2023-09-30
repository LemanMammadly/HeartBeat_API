using AutoMapper;
using SerenityHospital.Business.Dtos.SettingDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Profiles;

public class SettingMappingProfile:Profile
{
    public SettingMappingProfile()
    {
        CreateMap<SettingUpdateDto, Setting>().ReverseMap();
        CreateMap<Setting, SettingDetailItemDto>().ReverseMap();
        CreateMap<SettingCreateDto, Setting>().ReverseMap();
    }
}

