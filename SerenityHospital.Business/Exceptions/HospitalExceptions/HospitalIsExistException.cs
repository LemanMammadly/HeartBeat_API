using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.HospitalExceptions;

public class HospitalIsExistException : Exception, IBaseException
{

    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public HospitalIsExistException()
    {
        ErrorMessage = "Hospital is already exist";
    }

    public HospitalIsExistException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

