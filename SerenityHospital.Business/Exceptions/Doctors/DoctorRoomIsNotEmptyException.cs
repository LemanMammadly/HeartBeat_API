using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Doctors;

public class DoctorRoomIsNotEmptyException:Exception,IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }


    public DoctorRoomIsNotEmptyException()
    {
        ErrorMessage = "Doctor Room Is Not Empty Exception";
    }

    public DoctorRoomIsNotEmptyException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

