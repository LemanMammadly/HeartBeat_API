using System;
using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Roles;

public class RoleCreatedFailedException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public RoleCreatedFailedException()
    {
        ErrorMessage = "Role created failed for some reasons";
    }

    public RoleCreatedFailedException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

