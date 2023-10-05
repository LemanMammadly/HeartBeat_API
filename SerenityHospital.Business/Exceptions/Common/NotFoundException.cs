using Microsoft.AspNetCore.Http;
using SerenityHospital.Core.Entities.Common;

namespace SerenityHospital.Business.Exceptions.Common;

public class NotFoundException<T> : Exception, IBaseException where T : class
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public NotFoundException()
    {
        ErrorMessage = typeof(T).Name + "not found";
    }

    public NotFoundException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

