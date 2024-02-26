using System.Net;
using Cohors.Contracts;
using Cohors.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Cohors.Filters;

/// <summary>
/// Represents a filter that handles exceptions thrown during the execution of a controller action or another filter.
/// </summary>
public class ExceptionFilter(ILogger<ExceptionFilter> logger) : IExceptionFilter
{
    /// <summary>
    /// List of handlers that can handle different types of exceptions.
    /// </summary>
    private readonly List<IExceptionHandler> _handlers =
    [
        new HttpExceptionHandler(),
        new NotFoundExceptionHandler()
    ];

    /// <summary>
    /// Called after an action has thrown an <see cref="System.Exception"/>.
    /// </summary>
    /// <param name="context">The <see cref="ExceptionContext"/>.</param>
    public void OnException(ExceptionContext context)
    {
        // Iterates over the exception handlers to find a handler that can handle the current exception.
        foreach (var handler in _handlers.Where(handler => handler.CanHandle(context.Exception)))
        {
            var (error, statusCode) = handler.Handle(context.Exception);
            context.Result = new JsonResult(error) { StatusCode = (int)statusCode };
            logger.LogError(context.Exception, "{message}", context.Exception.Message);
            return;
        }
        
        // If no handler is found, a default error is returned.
        var defaultError = new ErrorDto(context.Exception.Message, HttpStatusCode.InternalServerError.ToString());
        context.Result = new JsonResult(defaultError) { StatusCode = (int)HttpStatusCode.InternalServerError };
        logger.LogError(context.Exception, "{message}", context.Exception.Message);
    }
}