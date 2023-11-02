using System;
using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Recipes;

public class ThisAppoinmentDateIsFutureException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public ThisAppoinmentDateIsFutureException()
    {
        ErrorMessage = "This Appoinment time is future.Recipe couln't be write";
    }

    public ThisAppoinmentDateIsFutureException(string? message) : base(message)
    {
        ErrorMessage = message;
    }

}

