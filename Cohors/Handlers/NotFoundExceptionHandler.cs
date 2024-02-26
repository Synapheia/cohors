using System.Net;
using Cohors.Contracts;
using Cohors.Exceptions;

namespace Cohors.Handlers;

public class NotFoundExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception exception)
    {
        return exception.GetType().IsGenericType && 
               exception.GetType().GetGenericTypeDefinition() == typeof(NotFoundException<>);
    }

    public (ErrorDto error, HttpStatusCode statusCode) Handle(Exception exception)
    {
        var error = new ErrorDto(exception.Message, HttpStatusCode.NotFound.ToString());
        return (error, HttpStatusCode.NotFound);
    }
}