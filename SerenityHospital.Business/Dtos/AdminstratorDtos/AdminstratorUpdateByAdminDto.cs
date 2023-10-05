using Microsoft.AspNetCore.Http;
using SerenityHospital.Core.Enums;

namespace SerenityHospital.Business.Dtos.AdminstratorDtos;

public record AdminstratorUpdateByAdminDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Description { get; set; }
    public int Age { get; set; }
    public DateTime StartDate { get; set; }
    public WorkStatus Status { get; set; }
    public decimal Salary { get; set; }
    public Gender Gender { get; set; }
    public IFormFile? ImageFile { get; set; }
}

