using Microsoft.AspNetCore.Mvc;
using Sample.Common.Helpers.Exceptions;

namespace Sample.API.Controllers;

[Route("errors/{code:int}")]
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }

    public IActionResult Error(int code)
    {
        var error = new ErrorResponse(code);
        _logger.LogError($"An exception occurred: {error.Message}");
        return new ObjectResult(error);
    }
}
