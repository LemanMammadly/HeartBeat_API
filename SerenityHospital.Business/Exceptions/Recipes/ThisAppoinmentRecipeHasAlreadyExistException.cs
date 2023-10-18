using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Recipes;

public class ThisAppoinmentRecipeHasAlreadyExistException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public ThisAppoinmentRecipeHasAlreadyExistException()
    {
        ErrorMessage = "This Appoinment Recipe Has Already Exist Exception";
    }

    public ThisAppoinmentRecipeHasAlreadyExistException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

