using Microsoft.AspNetCore.Http;

namespace Cohors.Contracts;

/// <summary>
///     ResponseDto class represents the response data object returned by the API.
/// </summary>
public class ResponseDto<T>
{
    public T? Payload { get; set; } = default(T);
    public bool Success { get; set; } = true;
    public string? Message { get; set; }
    public string? StatusCode { get; set; } = StatusCodes.Status200OK.ToString();

}




