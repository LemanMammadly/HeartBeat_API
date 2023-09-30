using System;
using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

public class SettingIsExistException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public SettingIsExistException()
    {
        ErrorMessage = "Setting is already exists";
    }

    public SettingIsExistException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

