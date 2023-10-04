using Microsoft.AspNetCore.Http;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Exceptions.Common;

public class RegisterFailedException<T> : Exception, IBaseException where T : AppUser
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public RegisterFailedException()
    {
        ErrorMessage = typeof(T).Name + " register failed for some reasons";
    }

    public RegisterFailedException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

