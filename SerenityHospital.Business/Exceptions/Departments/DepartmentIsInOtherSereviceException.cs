using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Departments;

public class DepartmentIsInOtherSereviceException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public DepartmentIsInOtherSereviceException()
    {
        ErrorMessage = "Department Is In Other Service";
    }

    public DepartmentIsInOtherSereviceException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

