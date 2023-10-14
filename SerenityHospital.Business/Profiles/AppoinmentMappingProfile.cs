using AutoMapper;
using SerenityHospital.Business.Dtos.AppoinmentDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Profiles;

public class AppoinmentMappingProfile:Profile
{
    public AppoinmentMappingProfile()
    {
        CreateMap<AppoinmentCreateDto, Appoinment>().ReverseMap();
        CreateMap<AppoinmentUpdateDto, Appoinment>().ReverseMap();
        CreateMap<Appoinment, AppoinmentListItemDto>().ReverseMap();
        CreateMap<Appoinment, AppoinmentDetailItemDto>().ReverseMap();
    }
}

