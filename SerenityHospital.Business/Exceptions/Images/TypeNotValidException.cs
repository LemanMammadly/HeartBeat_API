using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Images;

public class TypeNotValidException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public TypeNotValidException()
    {
        ErrorMessage = "Type Not Valid Exception";
    }

    public TypeNotValidException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

