using System.Net;

namespace Sample.Common.Helpers.Exceptions;

public class ValidationErrors : ErrorResponse {
    public ValidationErrors() {
        StatusCode = (int)HttpStatusCode.BadRequest;
        Message = "Bad Request";
    }

    public IEnumerable<string>? Errors { get; set; }
}
