using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Tokens;

public class RefreshTokenExpiresIsOldException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status419AuthenticationTimeout;

    public string ErrorMessage { get; }

    public RefreshTokenExpiresIsOldException()
    {
        ErrorMessage = "Refresh token expires date is old";
    }

    public RefreshTokenExpiresIsOldException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

