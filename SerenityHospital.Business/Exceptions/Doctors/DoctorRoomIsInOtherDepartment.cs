using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Doctors;

public class DoctorRoomIsInOtherDepartment:Exception,IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }


    public DoctorRoomIsInOtherDepartment()
    {
        ErrorMessage = "Doctor Room Is In Other";
    }

    public DoctorRoomIsInOtherDepartment(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

