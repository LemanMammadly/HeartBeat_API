using AutoMapper;
using SerenityHospital.Business.Dtos.ServiceDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Profiles;

public class ServiceMappingProfile:Profile
{
    public ServiceMappingProfile()
    {
        CreateMap<ServiceCreateDto, Service>().ReverseMap();
        CreateMap<ServiceUpdateDto, Service>().ReverseMap();
        CreateMap<Service, ServiceListItemDto>().ReverseMap();
        CreateMap<Service, ServiceDetailItemDto>().ReverseMap();
    }
}

