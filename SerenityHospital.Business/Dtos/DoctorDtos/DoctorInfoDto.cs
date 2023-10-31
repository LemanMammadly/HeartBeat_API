using SerenityHospital.Business.Dtos.DepartmentDtos;
using SerenityHospital.Business.Dtos.PositionDtos;
using SerenityHospital.Core.Enums;

namespace SerenityHospital.Business.Dtos.DoctorDtos;

public record DoctorInfoDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string UserName { get; set; }
    public string? ImageUrl { get; set; }
    public string Email { get; set; }
    public DoctorAvailabilityStatus AvailabilityStatus { get; set; }
    public PositionInfoDto Position { get; set; }
    public DepartmentInfoDto Department { get; set; }
}



