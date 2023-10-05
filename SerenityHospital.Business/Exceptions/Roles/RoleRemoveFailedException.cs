using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Roles;

public class RoleRemoveFailedException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public RoleRemoveFailedException()
    {
        ErrorMessage = "Role remove failed for some reasons";
    }

    public RoleRemoveFailedException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

