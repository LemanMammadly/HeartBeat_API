using AutoMapper;
using SerenityHospital.Business.Dtos.HospitalDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Profiles;

public class HospitalMappingProfile:Profile
{
    public HospitalMappingProfile()
    {
        CreateMap<HospitalUpdateDto, Hospital>().ReverseMap();
        CreateMap<HospitalCreateDto, Hospital>().ReverseMap();
        CreateMap<Hospital, HospitalDetailItemDto>().ReverseMap();
    }
}

