using SerenityHospital.Business.Dtos.DepartmentDtos;
using SerenityHospital.Core.Enums;

namespace SerenityHospital.Business.Dtos.NurseDtos;

public record NurseDetailItemDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
    public string UserName { get; set; }
    public decimal Salary { get; set; }
    public Gender Gender { get; set; }
    public WorkStatus Status { get; set; }
    public string Email { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime StartWork { get; set; }
    public DateTime EndWork { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsDeleted { get; set; }
    public DepartmentInfoDto Department { get; set; }
    public IEnumerable<string> Roles { get; set; }
}


