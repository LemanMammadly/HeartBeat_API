using SerenityHospital.Business.Dtos.DepartmentDtos;
using SerenityHospital.Business.Dtos.PositionDtos;

namespace SerenityHospital.Business.Dtos.DoctorDtos;

public record DoctorListItemDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string UserName { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime? EndDate { get; set; }
    public PositionInfoDto Position { get; set; }
    public DepartmentInfoDto Department { get; set; }
    public IEnumerable<string> Roles { get; set; }

}

