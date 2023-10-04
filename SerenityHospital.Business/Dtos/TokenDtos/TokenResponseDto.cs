namespace SerenityHospital.Business.Dtos.TokenDtos;

public record TokenResponseDto
{
    public string Token { get; set; }
    public string Username { get; set; }
    public DateTime Expires { get; set; }
}


