using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.PatientRooms;

public class PatientRoomCapacityIsFullException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public PatientRoomCapacityIsFullException()
    {
        ErrorMessage = "Patient Room Capacity is Full";
    }

    public PatientRoomCapacityIsFullException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

