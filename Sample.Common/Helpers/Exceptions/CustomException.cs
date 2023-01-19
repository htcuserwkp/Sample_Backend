using System.Net;

namespace Sample.Common.Helpers.Exceptions;

public class CustomException : Exception
{
    public string CustomMessage { get; set; } = null!;

    public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.BadRequest;
}
