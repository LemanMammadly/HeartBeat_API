using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Doctors;

public class DoctorHasAppoinmentException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public DoctorHasAppoinmentException()
    {
        ErrorMessage = "Doctor Has Appoinment Exception";
    }

    public DoctorHasAppoinmentException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

