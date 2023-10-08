using SerenityHospital.Business.Dtos.DoctorDtos;
using SerenityHospital.Business.Dtos.PatientRoomDtos;
using SerenityHospital.Business.Dtos.PositionDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Dtos.DepartmentDtos;

public record DepartmentListItemDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconUrl { get; set; }
    public int ServiceId { get; set; }
    public bool IsDeleted { get; set; }
    public IEnumerable<PatientRoomListItemDto> PatientRooms { get; set; }
    public IEnumerable<DoctorInfoDto> Doctors { get; set; }
    public IEnumerable<PositionInfoDto> Positions { get; set; }
}


