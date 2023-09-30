using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Exceptions.Common;

namespace SerenityHospital.Business.Exceptions.Images;

public class ImagePathIsNullOrWhiteSpaceException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public ImagePathIsNullOrWhiteSpaceException()
    {
        ErrorMessage = "Image Path Is Null Or White Space Exception";
    }

    public ImagePathIsNullOrWhiteSpaceException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

