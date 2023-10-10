using Microsoft.AspNetCore.Http;

namespace SerenityHospital.Business.Exceptions.Common;

public class LogoutFaileException<T> : Exception, IBaseException where T:class
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public LogoutFaileException()
    {
        ErrorMessage = typeof(T).Name + " Logout Failed Exception";
    }

    public LogoutFaileException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

