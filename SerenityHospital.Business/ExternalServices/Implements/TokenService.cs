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
    readonly UserManager<Doctor> _doctorUserManager;
    readonly UserManager<Patient> _patientUserManager;
    readonly UserManager<Nurse> _nurseUserManager;

    public TokenService(IConfiguration configuration, UserManager<Adminstrator> userManager, UserManager<Doctor> doctorUserManager, UserManager<Patient> patientUserManager, UserManager<Nurse> nurseUserManager)
    {
        _configuration = configuration;
        _adminstratorUserManager = userManager;
        _doctorUserManager = doctorUserManager;
        _patientUserManager = patientUserManager;
        _nurseUserManager = nurseUserManager;
    }

    public string CreateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }

    public TokenResponseDto CreateAdminstratorToken(Adminstrator adminstrator, int expires = 60)
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

    public TokenResponseDto CreateDoctorToken(Doctor doctor, int expires = 60)
    {

        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name,doctor.UserName),
            new Claim(ClaimTypes.NameIdentifier,doctor.Id),
            new Claim(ClaimTypes.Email,doctor.Email),
            new Claim(ClaimTypes.GivenName,doctor.Name),
            new Claim(ClaimTypes.Surname,doctor.Surname),
        };

        foreach (var userRole in _doctorUserManager.GetRolesAsync(doctor).Result)
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

        JwtSecurityTokenHandler jwtSecurityToken = new JwtSecurityTokenHandler();
        string token = jwtSecurityToken.WriteToken(jwtSecurity);

        string refreshToken = CreateRefreshToken();
        var refreshTokenExpires = jwtSecurity.ValidTo.AddMinutes(expires / 3);
        doctor.RefreshToken = refreshToken;
        doctor.RefreshTokenExpiresDate = refreshTokenExpires;
        _doctorUserManager.UpdateAsync(doctor).Wait();
        return new()
        {
            Token = token,
            Expires = jwtSecurity.ValidTo,
            Username = doctor.UserName,
            RefreshToken = refreshToken,
            RefreshTokenExpires = refreshTokenExpires
        };
    }

    public TokenResponseDto CreatePatientToken(Patient patient, int expires = 60)
    {

        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name,patient.UserName),
            new Claim(ClaimTypes.NameIdentifier,patient.Id),
            new Claim(ClaimTypes.Email,patient.Email),
            new Claim(ClaimTypes.GivenName,patient.Name),
            new Claim(ClaimTypes.Surname,patient.Surname)
        };

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));

        SigningCredentials credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtSecurity = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            DateTime.UtcNow.AddHours(4),
            DateTime.UtcNow.AddHours(4).AddMinutes(expires),
            credentials);

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        string token = tokenHandler.WriteToken(jwtSecurity);

        string refreshToken = CreateRefreshToken();
        var refreshTokenExpires = jwtSecurity.ValidTo.AddMinutes(expires / 3);
        patient.RefreshToken = refreshToken;
        patient.RefreshTokenExpiresDate = refreshTokenExpires;
        _patientUserManager.UpdateAsync(patient).Wait();

        return new()
        {
            Token = token,
            Expires = jwtSecurity.ValidTo,
            Username = patient.UserName,
            RefreshToken = refreshToken,
            RefreshTokenExpires = refreshTokenExpires
        };
    }

    public TokenResponseDto CreateNurseToken(Nurse nurse, int expires = 60)
    {

        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name,nurse.UserName),
            new Claim(ClaimTypes.NameIdentifier,nurse.Id),
            new Claim(ClaimTypes.Email,nurse.Email),
            new Claim(ClaimTypes.GivenName,nurse.Name),
            new Claim(ClaimTypes.Surname,nurse.Surname),
        };


        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));

        SigningCredentials credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            DateTime.UtcNow.AddHours(4),
            DateTime.UtcNow.AddHours(4).AddMinutes(expires),
            credentials);

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        string token = tokenHandler.WriteToken(jwtSecurityToken);
        string refreshToken = CreateRefreshToken();
        var refreshTokenExpires = jwtSecurityToken.ValidTo.AddMinutes(expires / 3);
        nurse.RefreshToken = refreshToken;
        nurse.RefreshTokenExpiresDate = refreshTokenExpires;
        _nurseUserManager.UpdateAsync(nurse).Wait();

        return new()
        {
            Token = token,
            Expires = jwtSecurityToken.ValidTo,
            Username = nurse.UserName,
            RefreshToken = refreshToken,
            RefreshTokenExpires = refreshTokenExpires
        };
    }
}

