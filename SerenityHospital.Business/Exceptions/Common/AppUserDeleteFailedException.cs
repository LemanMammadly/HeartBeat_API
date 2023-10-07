using Microsoft.AspNetCore.Http;

namespace SerenityHospital.Business.Exceptions.Common;

public class AppUserDeleteFailedException<T> : Exception, IBaseException where T : class
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }


    public AppUserDeleteFailedException()
    {
        ErrorMessage = typeof(T).Name + " failed deleted";
    }

    public AppUserDeleteFailedException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

