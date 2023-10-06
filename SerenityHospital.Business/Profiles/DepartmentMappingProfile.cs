using AutoMapper;
using SerenityHospital.Business.Dtos.DepartmentDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Profiles;

public class DepartmentMappingProfile:Profile
{
    public DepartmentMappingProfile()
    {
        CreateMap<DepartmentCreateDto, Department>().ReverseMap();
        CreateMap<DepartmentUpdateDto, Department>().ReverseMap();
        CreateMap<Department, DepartmentDetailItemDto>().ReverseMap();
        CreateMap<Department, DepartmentListItemDto>().ReverseMap();
        CreateMap<Department, DepartmentInfoDto>().ReverseMap();
    }
}

