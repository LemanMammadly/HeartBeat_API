using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Appoinments;

public class AppoinmentCouldntBeDeleteException:Exception,IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public AppoinmentCouldntBeDeleteException()
    {
        ErrorMessage = "Appoinment Couldnt Delete";
    }

    public AppoinmentCouldntBeDeleteException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

