using System.Net;

namespace Sample.Common.Helpers.Exceptions;

public class ValidationErrorResponse : ErrorResponse {
    public ValidationErrorResponse() {
        StatusCode = (int)HttpStatusCode.BadRequest;
        Message = GetDefaultMessage(StatusCode);
    }

    public IEnumerable<string>? Errors { get; set; }
}
