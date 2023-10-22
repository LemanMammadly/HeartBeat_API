using AutoMapper;
using SerenityHospital.Business.Dtos.AdminDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Profiles;

public class AdminMappingProfile:Profile
{
    public AdminMappingProfile()
    {
        CreateMap<AdminCreateDto, Admin>().ReverseMap();
        CreateMap<AdminUpdateDto, Admin>().ReverseMap();
        CreateMap<AdminUpdateBySuperAdminDto, Admin>().ReverseMap();
        CreateMap<Admin, AdminDetailItemDto>().ReverseMap();
        CreateMap<Admin, AdminListItemDto>().ReverseMap();
    }
}

