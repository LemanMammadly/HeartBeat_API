using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Doctors;

public class DoctorHavAlreadyRoomException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }


    public DoctorHavAlreadyRoomException()
    {
        ErrorMessage = "Doctor Have Already Room ";
    }

    public DoctorHavAlreadyRoomException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

