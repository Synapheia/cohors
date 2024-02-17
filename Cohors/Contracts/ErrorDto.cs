namespace Cohors.Contracts;

/// <summary>
///     Represents an error response DTO.
/// </summary>
public class ErrorDto : ResponseDto<ErrorDto>
{
    public ErrorDto(string message, string statusCode)
    {
        Success = false;
        Message = message;
        StatusCode = statusCode;
    }
}