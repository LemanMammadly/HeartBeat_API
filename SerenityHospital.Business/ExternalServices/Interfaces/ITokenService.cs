using SerenityHospital.Business.Dtos.TokenDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.ExternalServices.Interfaces;

public interface ITokenService
{
    TokenResponseDto CreateToken(Adminstrator adminstrator, int expires = 60);
}

