using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Confirms;

public class EmailConfirmationFailedException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public EmailConfirmationFailedException()
    {
        ErrorMessage = "Email confirmation failed";
    }

    public EmailConfirmationFailedException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

