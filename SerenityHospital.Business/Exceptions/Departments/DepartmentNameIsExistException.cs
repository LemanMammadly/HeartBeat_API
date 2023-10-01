using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Departments;

public class DepartmentNameIsExistException : Exception, IBaseException
{

    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public DepartmentNameIsExistException()
    {
        ErrorMessage = "Department name is already exist";
    }

    public DepartmentNameIsExistException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

