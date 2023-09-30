using System;
using Microsoft.AspNetCore.Http;
using SerenityHospital.Core.Entities.Common;

namespace SerenityHospital.Business.Exceptions.Common;

public class NegativeIdException<T> : Exception, IBaseException where T : BaseEntity
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public NegativeIdException()
    {
        ErrorMessage = typeof(T).Name + "Id must be greater than 0";
    }

    public NegativeIdException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

