using System.Net;

namespace Sample.Common.Helpers.Formats;

public class Formats
{
    //TODO:
    public static string FormatHttpStatusMessage(HttpStatusCode statusCode) {
        var statusString = statusCode.ToString();
        return $"{char.ToUpper(statusString[0])}{statusString[1..].ToLower()}";
    }
}