using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Patients;

public class PatientHasPatientRoomException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public PatientHasPatientRoomException()
    {
        ErrorMessage = "Patient Has PatientRoom Exception";
    }

    public PatientHasPatientRoomException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

