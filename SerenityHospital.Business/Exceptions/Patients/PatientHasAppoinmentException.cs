using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Patients;

public class PatientHasAppoinmentException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public PatientHasAppoinmentException()
    {
        ErrorMessage = "Patient Has Appoinment Exception";
    }

    public PatientHasAppoinmentException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

