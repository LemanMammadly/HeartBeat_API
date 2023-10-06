using AutoMapper;
using SerenityHospital.Business.Dtos.AdminstratorDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Profiles;

public class AdminstratorMappingProfile:Profile
{
    public AdminstratorMappingProfile()
    {
        CreateMap<CreateAdminstratorDto, Adminstrator>().ReverseMap();
        CreateMap<AdminstratorUpdateDto, Adminstrator>().ReverseMap();
        CreateMap<Adminstrator, AdminstratorListItemDto>().ReverseMap();
        CreateMap<Adminstrator, AdminstratorDetailItemDto>().ReverseMap();
        CreateMap<AdminstratorUpdateByAdminDto, Adminstrator>().ReverseMap();
    }
}

