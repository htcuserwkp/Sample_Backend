using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Sample.API.Controllers.SampleControllers;

public class WeatherForecastController : BaseApiController
{
    private static readonly string[] Summaries = {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IMemoryCache _cache;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IMemoryCache cache)
    {
        _logger = logger;
        _cache = cache;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get([FromQuery] int noOfDays = 5)
    {
        return Enumerable.Range(0, noOfDays).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
            .ToArray();
    }

    [HttpGet("ForTime")]
    public IActionResult GetForecastByTime([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (startDate >= endDate)
            {
                return BadRequest("Start date must be before end date.");
            }

            var cacheKey = $"WeatherForecast_{startDate}_{endDate}";

            var difference = (endDate - startDate).Days;
            if (_cache.TryGetValue(cacheKey, out IEnumerable<WeatherForecast>? forecasts)) return Ok(forecasts);
            forecasts = Enumerable.Range(1, difference).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(startDate.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep in cache for this time, reset time if accessed.
                .SetSlidingExpiration(TimeSpan.FromSeconds(60));

            _cache.Set(cacheKey, forecasts, cacheEntryOptions);

            return Ok(forecasts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting forecast");

            return StatusCode(500, "An error occurred while getting the forecast. Please try again later.");
        }
    }
}

public class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}