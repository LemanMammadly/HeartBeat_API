using AutoMapper;
using SerenityHospital.Business.Dtos.DoctorDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Profiles;

public class DoctorMappingProfile:Profile
{
    public DoctorMappingProfile()
    {
        CreateMap<DoctorCreateDto, Nurse>().ReverseMap();
        CreateMap<DoctorUpdateDto, Nurse>().ReverseMap();
        CreateMap<DoctorUpdateByAdminDto, Nurse>().ReverseMap();
        CreateMap<Nurse, DoctorInfoDto>().ReverseMap();
        CreateMap<Nurse, DoctorDetailItemDto>().ReverseMap();
        CreateMap<Nurse, DoctorListItemDto>().ReverseMap();
        CreateMap<Nurse, AppoinmentAsDoctorDto>().ReverseMap();
    }
}

