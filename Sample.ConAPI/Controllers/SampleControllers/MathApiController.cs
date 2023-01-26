using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Sample.API.Controllers.SampleControllers;

public class MathApiController : BaseApiController
{
    private readonly ILogger<MathApiController> _logger;

    public MathApiController(ILogger<MathApiController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult IsPrime([FromQuery][Range(1, long.MaxValue)] long number)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid input provided. {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            if (number == 2) return Ok(true);
            if (number % 2 == 0) return Ok(false);

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (var i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0) return Ok(false);
            }

            return Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while checking prime number");
            return StatusCode(500, "An error occurred. Please try again later.");
        }
    }

}