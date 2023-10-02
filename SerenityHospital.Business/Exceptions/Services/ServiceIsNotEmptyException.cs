using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Services;

public class ServiceIsNotEmptyException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public ServiceIsNotEmptyException()
    {
        ErrorMessage = "Service Is Not Empty";
    }

    public ServiceIsNotEmptyException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

