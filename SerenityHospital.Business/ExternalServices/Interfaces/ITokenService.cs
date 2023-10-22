using SerenityHospital.Business.Dtos.TokenDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.ExternalServices.Interfaces;

public interface ITokenService
{
    TokenResponseDto CreateAdminstratorToken(Adminstrator adminstrator, int expires = 60);
    TokenResponseDto CreateDoctorToken(Doctor doctor, int expires = 60);
    TokenResponseDto CreatePatientToken(Patient patient, int expires = 60);
    TokenResponseDto CreateNurseToken(Nurse nurse, int expires = 60);
    TokenResponseDto CreateAdminToken(Admin admin, int expires = 60);
    string CreateRefreshToken();
}

