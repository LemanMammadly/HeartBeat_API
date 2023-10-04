using Microsoft.AspNetCore.Http;
using SerenityHospital.Core.Entities;
using SerenityHospital.Core.Entities.Common;

namespace SerenityHospital.Business.Exceptions.Common;

public class AppUserIsAlreadyExistException<T> : Exception, IBaseException where T : AppUser
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }


    public AppUserIsAlreadyExistException()
    {
        ErrorMessage = typeof(T).Name + " user is already exist";
    }

    public AppUserIsAlreadyExistException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

