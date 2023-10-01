using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Services;

public class ServiceNameIsExistException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public ServiceNameIsExistException()
    {
        ErrorMessage = "Service name is already exist";
    }

    public ServiceNameIsExistException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

