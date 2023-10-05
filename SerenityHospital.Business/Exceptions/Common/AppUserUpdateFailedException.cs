using Microsoft.AspNetCore.Http;

namespace SerenityHospital.Business.Exceptions.Common;

public class AppUserUpdateFailedException<T> : Exception, IBaseException where T : class
{
    public int StatusCode =>StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public AppUserUpdateFailedException()
    {
        ErrorMessage = typeof(T).Name + " update failed for some reasons";
    }

    public AppUserUpdateFailedException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

