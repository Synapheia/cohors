using System.Net;
using Cohors.Contracts;

namespace Cohors.Handlers;

public interface IExceptionHandler
{
    bool CanHandle(Exception exception);
    (ErrorDto error, HttpStatusCode statusCode) Handle(Exception exception);
}