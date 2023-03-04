using System.Text.Json;

namespace Sample.Common.Helpers.Exceptions;

public class ErrorResponse
{
    public ErrorResponse()
    {
    }

    public ErrorResponse(int statusCode, string? message = null)
    {
        StatusCode = statusCode;
        Message = message ?? GetDefaultMessage(statusCode);
    }

    public int StatusCode { get; set; }
    public string Message { get; set; } = null!;

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    protected static string GetDefaultMessage(int statusCode) => statusCode switch
    {
        400 => "Bad Request",
        401 => "Not Authorized",
        403 => "Access Forbidden",
        404 => "Not Found",
        405 => "Not Allowed",
        500 => "Internal Server Error",
        503 => "Service Unavailable",
        _ => string.Empty,
    };
}
