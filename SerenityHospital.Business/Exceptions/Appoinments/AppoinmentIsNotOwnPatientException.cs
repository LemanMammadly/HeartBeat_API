using System;
using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Appoinments
{
	public class AppoinmentIsNotOwnPatientException:Exception,IBaseException
	{
        public int StatusCode => StatusCodes.Status400BadRequest;

        public string ErrorMessage { get; }

        public AppoinmentIsNotOwnPatientException()
        {
            ErrorMessage = "Appoinment Is Not Own this Patient";
        }

        public AppoinmentIsNotOwnPatientException(string? message) : base(message)
        {
            ErrorMessage = message;
        }
    }
}

