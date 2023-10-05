using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Roles;

public class RoleUpdateFailedException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public RoleUpdateFailedException()
    {
        ErrorMessage = "Role update failed some reasosns";
    }

    public RoleUpdateFailedException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

