using System.Net;
using Cohors.Contracts;
using Cohors.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Cohors.Filters;

/// <summary>
///     Filter used to handle exceptions that occur during the execution of an action method.
/// </summary>
public class ExceptionFilter(ILogger<ExceptionFilter> logger) : IExceptionFilter
{
    /// <summary>
    ///     Handles exceptions that occur during the execution of an action.
    ///     This method checks if the exception is an HttpException, and if so, it creates an ErrorDto object with the error
    ///     message and status
    ///     code from the exception.
    ///     The ErrorDto object is then returned as a JsonResult with the corresponding status code.
    ///     If the exception is not an HttpException, a new ErrorDto object is created with the exception message and the
    ///     status
    ///     code set to
    ///     InternalServerError.
    ///     The ErrorDto object is again returned as a JsonResult with the corresponding status code.
    ///     Additionally, a log message is written to the logger with the warning level if the exception is an HttpException,
    ///     or with the
    ///     error level if it is not.
    /// </summary>
    /// <param name="context">The ExceptionContext object that contains information about the exception and the current action.</param>
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is HttpException httpException)
        {
            var error = new ErrorDto(httpException.Message, httpException.StatusCode.ToString());
            context.Result = new JsonResult(error) { StatusCode = (int)httpException.StatusCode };
            logger.LogWarning("{message}", context.Exception.Message);
        }
        else
        {
            var error = new ErrorDto(context.Exception.Message, HttpStatusCode.InternalServerError.ToString());
            context.Result = new JsonResult(error) { StatusCode = (int)HttpStatusCode.InternalServerError };
            logger.LogError(context.Exception, "{message}", context.Exception.Message);
        }
    }
}