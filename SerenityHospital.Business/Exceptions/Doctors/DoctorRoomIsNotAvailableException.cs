using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Doctors;

public class DoctorRoomIsNotAvailableException:Exception,IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }


    public DoctorRoomIsNotAvailableException()
    {
        ErrorMessage = "Doctor Room Is Not Available";
    }

    public DoctorRoomIsNotAvailableException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

