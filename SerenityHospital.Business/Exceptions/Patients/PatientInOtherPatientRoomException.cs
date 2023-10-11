using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Patients;

public class PatientInOtherPatientRoomException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public PatientInOtherPatientRoomException()
    {
        ErrorMessage = "Patient In Other Patient Room";
    }

    public PatientInOtherPatientRoomException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

