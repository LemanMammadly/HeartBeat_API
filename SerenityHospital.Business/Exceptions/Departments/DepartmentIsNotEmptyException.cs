using System;
using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Departments;

public class DepartmentIsNotEmptyException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public DepartmentIsNotEmptyException()
    {
        ErrorMessage = "Department is not empty";
    }

    public DepartmentIsNotEmptyException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

