using SerenityHospital.Business.Dtos.DepartmentDtos;

namespace SerenityHospital.Business.Dtos.NurseDtos;

public record NurseDetailItemDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime StartWork { get; set; }
    public DateTime EndWork { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsDeleted { get; set; }
    public DepartmentInfoDto Department { get; set; }
    public IEnumerable<string> Roles { get; set; }
}

