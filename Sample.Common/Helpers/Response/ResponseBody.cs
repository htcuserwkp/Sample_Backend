using System.Net;

namespace Sample.Common.Helpers.Response;

public class ResponseBody<TDataType>
{
    public ResponseBody()
    {
        Status = HttpStatusCode.OK;
    }

    public HttpStatusCode Status { get; set; }
    public IEnumerable<string>? Warnings { get; set; }
    public string? Message { get; set; }
    public TDataType? Data { get; set; }
}
