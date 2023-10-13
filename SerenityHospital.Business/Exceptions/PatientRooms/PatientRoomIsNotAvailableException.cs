using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.PatientRooms;

public class PatientRoomIsNotAvailableException:Exception,IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }


    public PatientRoomIsNotAvailableException()
    {
        ErrorMessage = "Patient Room Is Not Available";
    }

    public PatientRoomIsNotAvailableException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

