using Microsoft.AspNetCore.Mvc;
using Sample.Business.FoodBusinessLogic;
using Sample.Common.Dtos.FoodDtos;
using Sample.Common.Helpers.Response;
using System.Text.Json;

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
    public async Task<ActionResult<ResponseBody<IEnumerable<FoodDto>>>> GetAllFood()
    {
        _logger.LogInformation("Get all Food request received.");
        var response = new ResponseBody<IEnumerable<FoodDto>>();

        var foods = await _foodService.GetAllFoodAsync().ConfigureAwait(false);

        response.Data = foods;
        response.Message = "Foods retrieved successfully";
        _logger.LogInformation(response.Message);

        return Ok(response);
    }

    // GET: api/Food-By-Id/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseBody<FoodDto>>> GetFood(long id)
    {
        _logger.LogInformation($"Get Food request received for ID: {id}.");
        var response = new ResponseBody<FoodDto>();

        var food = await _foodService.GetByIdAsync(id).ConfigureAwait(false);
        response.Data = food;
        response.Message = $"Food retrieved successfully";
        _logger.LogInformation(response.Message);

        return Ok(response);
    }

    // POST: api/Add-Food
    [HttpPost("add")]
    public async Task<IActionResult> SaveFood([FromBody] FoodAddDto foodDetails)
    {
        _logger.LogInformation($"Add request received for Food: {JsonSerializer.Serialize(foodDetails)}.");
        var response = new ResponseBody<string>();

        var status = await _foodService.AddFoodAsync(foodDetails).ConfigureAwait(false);

        response.Message = status;
        _logger.LogInformation(response.Message);

        return Ok(response);
    }

    // PUT: api/Food-Update
    [HttpPut("update")]
    public async Task<IActionResult> UpdateFood([FromBody] FoodDto foodDetails)
    {
        _logger.LogInformation($"Update request received for Food: {JsonSerializer.Serialize(foodDetails)}.");
        var response = new ResponseBody<string>();

        var status = await _foodService.UpdateFoodAsync(foodDetails).ConfigureAwait(false);

        response.Message = status;
        _logger.LogInformation(response.Message);

        return Ok(response);
    }

    // DELETE: api/Food-Delete
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFood(long id)
    {
        _logger.LogInformation($"Delete request received for Food ID: {id}.");
        var response = new ResponseBody<string>();

        var status = await _foodService.DeleteFoodAsync(id).ConfigureAwait(false);

        response.Message = status;
        _logger.LogInformation(response.Message);

        return Ok(response);
    }
}