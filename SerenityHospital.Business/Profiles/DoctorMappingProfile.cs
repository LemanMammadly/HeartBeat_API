using AutoMapper;
using SerenityHospital.Business.Dtos.DoctorDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Profiles;

public class DoctorMappingProfile:Profile
{
    public DoctorMappingProfile()
    {
        CreateMap<DoctorCreateDto, Doctor>().ReverseMap();
    }
}

