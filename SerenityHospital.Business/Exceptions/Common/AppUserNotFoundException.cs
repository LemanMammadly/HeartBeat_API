using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Exceptions.Common;

public class AppUserNotFoundException<T>:Exception,IBaseException where T : AppUser
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public AppUserNotFoundException()
    {
        ErrorMessage = typeof(T).Name +  " user not found";
    }

    public AppUserNotFoundException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

