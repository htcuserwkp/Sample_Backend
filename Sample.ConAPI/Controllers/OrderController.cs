using Microsoft.AspNetCore.Mvc;
using Sample.Common.Helpers.Response;
using System.Text.Json;
using Sample.Business.Dtos.OrderDtos;
using Sample.Business.Services.OrderBusinessLogic;

namespace Sample.API.Controllers;

public class OrderController : BaseApiController
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderController> _logger;

    public OrderController(IOrderService orderService, ILogger<OrderController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }
    //GET: api/All-Orders
    [HttpGet("get-orders")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
    {
        _logger.LogInformation($"Get all Orders request received.");
        var response = new ResponseBody<IEnumerable<OrderDto>>();

        var orders = await _orderService.GetAllAsync().ConfigureAwait(false);

        response.Data = orders;
        response.Message = "Orders for customer retrieved successfully";
        _logger.LogInformation(response.Message);

        return Ok(response);
    }

    [HttpGet("GetByCustomer")]
    public async Task<ActionResult<ResponseBody<IEnumerable<OrderDto>>>> GetByCustomer(long customerId)
    {
        _logger.LogInformation($"Get all Orders for Customer ID:{customerId}, request received.");
        var response = new ResponseBody<IEnumerable<OrderDto>>();

        var orders = await _orderService.GetByCustomerAsync(customerId).ConfigureAwait(false);

        response.Data = orders;
        response.Message = "Orders for customer retrieved successfully";
        _logger.LogInformation(response.Message);

        return Ok(response);
    }

    [HttpGet("get-order/{id:long}")]
    public async Task<ActionResult<OrderDto>> GetById(long id)
    {
        _logger.LogInformation($"Get Order for ID:{id}, request received.");
        var response = new ResponseBody<OrderDto>();

        var order = await _orderService.GetByIdAsync(id).ConfigureAwait(false);

        response.Data = order;
        response.Message = "Order details retrieved successfully";
        _logger.LogInformation(response.Message);

        return Ok(response);
    }

    [HttpGet("GetByOrderNumber")]
    public async Task<ActionResult<OrderDto>> GetByOrderNumber(string orderNumber)
    {
        _logger.LogInformation($"Get Order for #{orderNumber}, request received.");
        var response = new ResponseBody<OrderDto>();

        var order = await _orderService.GetByOrderNumber(orderNumber).ConfigureAwait(false);

        response.Data = order;
        response.Message = "Order details retrieved successfully";
        _logger.LogInformation(response.Message);

        return Ok(response);
    }

    [HttpPost("PlaceOrder")]
    public async Task<IActionResult> PlaceOrder([FromBody] OrderAddDto orderDetails)
    {
        _logger.LogInformation($"Place order request received for Order: {JsonSerializer.Serialize(orderDetails)}.");
        var response = new ResponseBody<string>();

        var status = await _orderService.PlaceOrderAsync(orderDetails).ConfigureAwait(false);

        response.Message = status;
        _logger.LogInformation(response.Message);

        return Ok(response);
    }
}