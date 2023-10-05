using Microsoft.AspNetCore.Identity;

namespace SerenityHospital.Business.Dtos.AdminstratorDtos;

public record AdminstratorListItemDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string UserName { get; set; }
    public string? ImageUrl { get; set; }
    public IEnumerable<string> Roles { get; set; }
}



 