using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Positions;

public class PositionIsNotEmptyException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public PositionIsNotEmptyException()
    {
        ErrorMessage = "Position Is Not Empty";
    }

    public PositionIsNotEmptyException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

