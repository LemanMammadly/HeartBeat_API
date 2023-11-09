﻿using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Appoinments;

public class ConflictingAppointmentException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public ConflictingAppointmentException()
    {
        ErrorMessage = "Doctor or you busy at this time";
    }

    public ConflictingAppointmentException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

