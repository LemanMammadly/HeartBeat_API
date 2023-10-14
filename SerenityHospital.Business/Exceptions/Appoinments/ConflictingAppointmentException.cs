using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Appoinments;

public class ConflictingAppointmentException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public ConflictingAppointmentException()
    {
        ErrorMessage = "Conflicting Appointment Exception";
    }

    public ConflictingAppointmentException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

