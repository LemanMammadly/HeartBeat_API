using Microsoft.AspNetCore.Http;
using SerenityHospital.Core.Entities.Common;

namespace SerenityHospital.Business.Exceptions.Common;

public class NameIsAlreadyExistException<T> : Exception, IBaseException where T : BaseEntity
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public NameIsAlreadyExistException()
    {
        ErrorMessage = typeof(T).Name + "name is already exist";
    }

    public NameIsAlreadyExistException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

