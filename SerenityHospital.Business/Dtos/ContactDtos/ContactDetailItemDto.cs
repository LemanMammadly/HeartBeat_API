namespace SerenityHospital.Business.Dtos.ContactDtos;

public record ContactDetailItemDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public DateTime Date { get; set; }
}

