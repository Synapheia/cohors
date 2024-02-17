using System.Net;

namespace Cohors.Exceptions;

/// <summary>
///     Represents an exception related to HTTP requests or responses.
/// </summary>
/// <param name="message"></param>
/// <param name="statusCode"></param>
public class HttpException(string message, HttpStatusCode statusCode) : Exception(message)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}