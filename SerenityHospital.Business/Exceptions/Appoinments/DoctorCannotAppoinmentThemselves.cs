using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Appoinments;

public class DoctorCannotAppoinmentThemselves : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public DoctorCannotAppoinmentThemselves()
    {
        ErrorMessage = "Doctors cannot make an appointment on their own";
    }

    public DoctorCannotAppoinmentThemselves(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

