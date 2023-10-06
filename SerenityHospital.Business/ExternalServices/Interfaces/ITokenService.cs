using SerenityHospital.Business.Dtos.TokenDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.ExternalServices.Interfaces;

public interface ITokenService
{
    TokenResponseDto CreateAdminstratorToken(Adminstrator adminstrator, int expires = 60);
    TokenResponseDto CreateDoctorToken(Doctor doctor, int expires = 60);
    string CreateRefreshToken();
}

