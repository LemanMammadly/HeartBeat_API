using AutoMapper;
using SerenityHospital.Business.Dtos.DoctorDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Profiles;

public class DoctorMappingProfile:Profile
{
    public DoctorMappingProfile()
    {
        CreateMap<DoctorCreateDto, Doctor>().ReverseMap();
        CreateMap<DoctorUpdateDto, Doctor>().ReverseMap();
        CreateMap<DoctorUpdateByAdminDto, Doctor>().ReverseMap();
        CreateMap<Doctor, DoctorInfoDto>().ReverseMap();
        CreateMap<Doctor, DoctorDetailItemDto>().ReverseMap();
        CreateMap<Doctor, DoctorListItemDto>().ReverseMap();
    }
}

