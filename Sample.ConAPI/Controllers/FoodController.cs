using Microsoft.AspNetCore.Mvc;
using Sample.Business.FoodBusinessLogic;
using Sample.Common.Dtos.FoodDtos;
using Sample.ConAPI.Controllers;

namespace Sample.API.Controllers;

public class FoodController : BaseApiController
{
    private readonly IFoodService _foodService;

    public FoodController(IFoodService foodService)
    {
        _foodService = foodService;
    }

    //GET: api/All-Food
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<FoodDto>>> GetAllFood()
    {
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