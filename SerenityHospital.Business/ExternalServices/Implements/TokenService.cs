using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SerenityHospital.Business.Dtos.TokenDtos;
using SerenityHospital.Business.ExternalServices.Interfaces;
using SerenityHospital.Core.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace SerenityHospital.Business.ExternalServices.Implements;

public class TokenService : ITokenService
{
    readonly IConfiguration _configuration;
    readonly UserManager<Adminstrator> _adminstratorUserManager;

    public TokenService(IConfiguration configuration, UserManager<Adminstrator> userManager)
    {
        _configuration = configuration;
        _adminstratorUserManager = userManager;
    }

    public string CreateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }

    public TokenResponseDto CreateToken(Adminstrator adminstrator, int expires = 60)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name,adminstrator.UserName),
            new Claim(ClaimTypes.NameIdentifier,adminstrator.Id),
            new Claim(ClaimTypes.Email,adminstrator.Email),
            new Claim(ClaimTypes.GivenName,adminstrator.Name),
            new Claim(ClaimTypes.Surname,adminstrator.Surname)
        };

        foreach (var userRole in _adminstratorUserManager.GetRolesAsync(adminstrator).Result)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));

        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtSecurity = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            DateTime.UtcNow.AddHours(4),
            DateTime.UtcNow.AddHours(4).AddMinutes(expires),
            credentials);

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        string token = tokenHandler.WriteToken(jwtSecurity);

        string refreshtoken = CreateRefreshToken();
        var refreshtokenExpires = jwtSecurity.ValidTo.AddMinutes(expires / 3);
        adminstrator.RefreshToken = refreshtoken;
        adminstrator.RefreshTokenExpiresDate = refreshtokenExpires;
        _adminstratorUserManager.UpdateAsync(adminstrator).Wait();
        return new()
        {
            Token = token,
            Expires = jwtSecurity.ValidTo,
            Username = adminstrator.UserName,
            RefreshToken=refreshtoken,
            RefreshTokenExpires=refreshtokenExpires
        };
    }
}

