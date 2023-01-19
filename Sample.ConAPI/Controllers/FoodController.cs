using Microsoft.AspNetCore.Mvc;
using Sample.Business.FoodBusinessLogic;
using Sample.Common.Dtos.FoodDtos;

namespace Sample.API.Controllers;

public class FoodController : BaseApiController
{
    private readonly IFoodService _foodService;
    private readonly ILogger<FoodController> _logger;

    public FoodController(IFoodService foodService, ILogger<FoodController> logger)
    {
        _foodService = foodService;
        _logger = logger;
    }

    //GET: api/All-Food
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<FoodDto>>> GetAllFood()
    {
        _logger.LogInformation("Get all Food request received.");
        var foods = await _foodService.GetAllFoodAsync().ConfigureAwait(false);

        return foods.Any() ? Ok(foods) : NotFound();
    }

    // POST: api/Add-Food
    [HttpPost("add")]
    public async Task<IActionResult> SaveFood([FromBody] FoodAddDto foodDetails)
    {
        var status = await _foodService.AddFoodAsync(foodDetails).ConfigureAwait(false);

        return Ok(status);
    }
}