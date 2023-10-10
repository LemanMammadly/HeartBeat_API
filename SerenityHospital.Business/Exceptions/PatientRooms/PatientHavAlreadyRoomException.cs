using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.PatientRooms;

public class PatientHavAlreadyRoomException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public PatientHavAlreadyRoomException()
    {
        ErrorMessage = "Patient Have Already Room";
    }

    public PatientHavAlreadyRoomException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

