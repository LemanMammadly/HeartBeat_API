using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Doctors;

public class DoctorRoomCapacityIsFullException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }


    public DoctorRoomCapacityIsFullException()
    {
        ErrorMessage = "Doctor Room Capacity Is Full";
    }

    public DoctorRoomCapacityIsFullException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

