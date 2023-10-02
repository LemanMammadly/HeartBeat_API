using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.PatientRooms;

public class ThisRoomNumberIsAlreadyExistException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public ThisRoomNumberIsAlreadyExistException()
    {
        ErrorMessage = "This Room Number Is Already Exist";
    }

    public ThisRoomNumberIsAlreadyExistException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

