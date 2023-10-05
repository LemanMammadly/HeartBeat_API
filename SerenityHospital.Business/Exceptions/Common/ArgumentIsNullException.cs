using Microsoft.AspNetCore.Http;

namespace SerenityHospital.Business.Exceptions.Common;

public class ArgumentIsNullException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }


    public ArgumentIsNullException()
    {
        ErrorMessage = "Argument is null";
    }

    public ArgumentIsNullException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

