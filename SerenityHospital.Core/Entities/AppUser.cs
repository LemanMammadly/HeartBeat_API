using Microsoft.AspNetCore.Identity;
using SerenityHospital.Core.Enums;

namespace SerenityHospital.Core.Entities;

public class AppUser:IdentityUser
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public string? ImageUrl { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresDate { get; set; }

}




