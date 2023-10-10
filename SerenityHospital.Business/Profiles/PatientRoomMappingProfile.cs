using AutoMapper;
using SerenityHospital.Business.Dtos.PatientRoomDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Profiles;

public class PatientRoomMappingProfile:Profile
{
    public PatientRoomMappingProfile()
    {
        CreateMap<PatientRoomCreateDto, PatientRoom>().ReverseMap();
        CreateMap<PatientRoomUpdateDto, PatientRoom>().ReverseMap();
        CreateMap<PatientRoom, PatientRoomListItemDto>().ReverseMap();
        CreateMap<PatientRoom, PatientRoomDetailItemDto>().ReverseMap();
        CreateMap<PatientRoom, PatientRoomInfoDto>().ReverseMap();
    }
}

