using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Roles;

public class RoleExistException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public RoleExistException()
    {
        ErrorMessage = "Role is already exist";
    }

    public RoleExistException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

