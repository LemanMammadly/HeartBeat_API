namespace SerenityHospital.Business.Dtos.AdminstratorDtos;

public record AdminstratorDetailItemDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string UserName { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime? EndDate { get; set; }
    public IEnumerable<string> Roles { get; set; }
}

