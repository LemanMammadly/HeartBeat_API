using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.PatientRooms;

public class PatientRoomNotEmptyException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public PatientRoomNotEmptyException()
    {
        ErrorMessage = "Patient Room Is Not Empty";
    }

    public PatientRoomNotEmptyException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

