namespace Sample.Common.Helpers.Exceptions;

public class DevErrorResponse : ErrorResponse
{
    public string ErrorPath { get; set; } = null!;
}
