using System;
using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Images;

public class SizeNotValidException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public SizeNotValidException()
    {
        ErrorMessage = "Size Not Valid Exception";
    }

    public SizeNotValidException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

