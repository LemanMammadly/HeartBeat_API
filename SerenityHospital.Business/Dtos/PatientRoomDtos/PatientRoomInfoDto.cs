using SerenityHospital.Core.Enums;

namespace SerenityHospital.Business.Dtos.PatientRoomDtos;

public record PatientRoomInfoDto
{
    public int Number { get; set; }
    public PatientRoomType Type { get; set; }
    public PatientRoomStatus Status { get; set; }
    public int Capacity { get; set; }
    public string ImageUrl { get; set; }
}

