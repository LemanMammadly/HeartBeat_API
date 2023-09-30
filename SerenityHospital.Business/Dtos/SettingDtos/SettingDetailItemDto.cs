namespace SerenityHospital.Business.Dtos.SettingDtos;

public record SettingDetailItemDto
{
    public int Id { get; set; }
    public string HeaderLogoUrl { get; set; }
    public string FooterLogoUrl { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}

