using Microsoft.AspNetCore.Http;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Exceptions.Common;

public class LoginFailedException<T> : Exception, IBaseException where T : AppUser
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public LoginFailedException()
    {
        ErrorMessage = typeof(T).Name + " Login failed some reasons";
    }

    public LoginFailedException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

