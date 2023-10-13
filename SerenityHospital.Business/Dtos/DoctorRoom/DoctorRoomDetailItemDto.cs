using SerenityHospital.Business.Dtos.DepartmentDtos;
using SerenityHospital.Business.Dtos.DoctorDtos;
using SerenityHospital.Core.Enums;

namespace SerenityHospital.Business.Dtos.DoctorRoom;

public record DoctorRoomDetailItemDto
{
    public int Id { get; set; }
    public int Number { get; set; }
    public DoctorRoomStatus DoctorRoomStatus { get; set; }
    public DepartmentInfoDto Department { get; set; }
    public DoctorInfoDto Doctor { get; set; }
    public bool IsDeleted { get; set; }
}



