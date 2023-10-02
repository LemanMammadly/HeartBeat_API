using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.PatientRooms;

public class PatientRoomInOtherDepartmentException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public PatientRoomInOtherDepartmentException()
    {
        ErrorMessage = "Patient Room In Other Department";
    }

    public PatientRoomInOtherDepartmentException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

