using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;

namespace Cohors.Handlers;
/// <summary>
/// A delegating handler that logs the details of HTTP requests and responses.
/// </summary>
public class LoggingDelegatingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingDelegatingHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingDelegatingHandler"/> class.
    /// </summary>
    /// <param name="logger">The logger to be used for logging.</param>
    public LoggingDelegatingHandler(ILogger<LoggingDelegatingHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
    /// </summary>
    /// <param name="request">The HTTP request message to send to the server.</param>
    /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
    /// <returns>The HTTP response message that the server sends back.</returns>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Log the request URL
            _logger.LogInformation("Sending request to {url}", request.RequestUri);

            var response = await base.SendAsync(request, cancellationToken);

            // Log the response status
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Received a successful response from {url}",
                    response.RequestMessage?.RequestUri);
            }
            else
            {
                _logger.LogWarning("Received a non-success status code {StatusCode} from {Url}",
                    (int)response.StatusCode, response.RequestMessage?.RequestUri);
            }

            return response;
        }
        catch (HttpRequestException ex)
            when (ex.InnerException is SocketException se && se.SocketErrorCode == SocketError.ConnectionRefused)
        {
            // Log the exception details
            var hostWithPort = request.RequestUri.IsDefaultPort
                ? request.RequestUri.DnsSafeHost
                : $"{request.RequestUri.DnsSafeHost}:{request.RequestUri.Port}";
            _logger.LogCritical(ex, "Unable to connect to {Host}. Please check the " +
                                   "configuration to ensure the correct URL for the service " +
                                   "has been configured.", hostWithPort);

        }

        // Return a BadGateway response
        return new HttpResponseMessage(HttpStatusCode.BadGateway)
        {
            RequestMessage = request
        };
    }
}