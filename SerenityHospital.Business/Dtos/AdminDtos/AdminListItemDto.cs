namespace SerenityHospital.Business.Dtos.AdminDtos;

public record AdminListItemDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public string? ImageUrl { get; set; }
    public IEnumerable<string> Roles { get; set; }
}

