using System.Net;
using Cohors.Contracts;
using Cohors.Exceptions;

namespace Cohors.Handlers;

public class HttpExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception exception)
    {
        return exception is HttpException;
    }

    public (ErrorDto error, HttpStatusCode statusCode) Handle(Exception exception)
    {
        var httpException = (HttpException)exception;
        return (new ErrorDto(
            httpException.Message, 
            httpException.StatusCode.ToString()),
            httpException.StatusCode);
    }
}