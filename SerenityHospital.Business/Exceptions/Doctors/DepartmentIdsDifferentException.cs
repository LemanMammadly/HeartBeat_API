using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Doctors;

public class DepartmentIdsDifferentException:Exception,IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }


    public DepartmentIdsDifferentException()
    {
        ErrorMessage = "Department Ids Different";
    }

    public DepartmentIdsDifferentException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

