using AutoMapper;
using SerenityHospital.Business.Dtos.PositionDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Profiles;

public class PositionMappingProfile:Profile
{
    public PositionMappingProfile()
    {
        CreateMap<PositionCreateDto, Position>().ReverseMap();
        CreateMap<PositionUpdateDto, Position>().ReverseMap();
        CreateMap<Position, PositionDetailItemDto>().ReverseMap();
        CreateMap<Position, PositionListItemDto>().ReverseMap();
    }
}

