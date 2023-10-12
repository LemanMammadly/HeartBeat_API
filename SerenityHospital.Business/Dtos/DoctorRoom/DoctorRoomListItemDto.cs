using SerenityHospital.Business.Dtos.DepartmentDtos;
using SerenityHospital.Business.Dtos.DoctorDtos;

namespace SerenityHospital.Business.Dtos.DoctorRoom;

public record DoctorRoomListItemDto
{
    public int Id { get; set; }
    public int Number { get; set; }
    public DepartmentInfoDto Department { get; set; }
    public DoctorInfoDto Doctor { get; set; }
    public bool IsDeleted { get; set; }
}

