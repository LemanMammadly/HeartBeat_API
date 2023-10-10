using SerenityHospital.Business.Dtos.PatientRoomDtos;

namespace SerenityHospital.Business.Dtos.PatientDtos;

public record AddPatientRoomDto
{
    public string Id { get; set; }
    public int RoomId { get; set; }
}


