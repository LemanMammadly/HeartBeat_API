using AutoMapper;
using SerenityHospital.Business.Dtos.DoctorRoom;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Profiles;

public class DoctorRoomMappingProfile:Profile
{
    public DoctorRoomMappingProfile()
    {
        CreateMap<DoctorRoomCreateDro, DoctorRoom>().ReverseMap();
        CreateMap<DoctorRoomUpdateDto, DoctorRoom>().ReverseMap();
        CreateMap<DoctorRoom, DoctorRoomDetailItemDto>().ReverseMap();
        CreateMap<DoctorRoom, DoctorRoomListItemDto>().ReverseMap();
    }
}

