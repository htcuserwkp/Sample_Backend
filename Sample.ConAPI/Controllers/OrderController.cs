using Microsoft.AspNetCore.Mvc;
using Sample.Common.Helpers.Response;
using Sample.Business.Dtos.OrderDtos;
using Sample.Business.Services.OrderBusinessLogic;
using Sample.Common.Helpers.Serializers;

namespace Sample.API.Controllers;

public class OrderController : BaseApiController {

    private readonly IOrderService _orderService;
    private readonly ILogger<OrderController> _logger;

    public OrderController(IOrderService orderService, ILogger<OrderController> logger) {
        _orderService = orderService;
        _logger = logger;
    }
    //GET: api/All-Orders
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll() {
        _logger.LogInformation($"Get all Orders request received.");
        var response = new ResponseBody<IEnumerable<OrderDto>>();

        var orders = await _orderService.GetAllOrdersAsync().ConfigureAwait(false);

        response.Data = orders;
        response.Message = "Orders for customer retrieved successfully";
        _logger.LogInformation(response.Message);

        return Ok(response);
    }

    [HttpGet("get-by-customer")]
    public async Task<ActionResult<ResponseBody<IEnumerable<OrderDto>>>> GetByCustomer(long customerId) {
        _logger.LogInformation($"Get all Orders for Customer ID:{customerId}, request received.");
        var response = new ResponseBody<IEnumerable<OrderDto>>();

        var orders = await _orderService.GetByCustomerAsync(customerId).ConfigureAwait(false);

        response.Data = orders;
        response.Message = "Orders for customer retrieved successfully";
        _logger.LogInformation(response.Message);

        return Ok(response);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<OrderDto>> GetById(long id) {
        _logger.LogInformation($"Get Order for ID:{id}, request received.");
        var response = new ResponseBody<OrderDto>();

        var order = await _orderService.GetByIdAsync(id).ConfigureAwait(false);

        response.Data = order;
        response.Message = "Order details retrieved successfully";
        _logger.LogInformation(response.Message);

        return Ok(response);
    }

    [HttpGet("get-by-number")]
    public async Task<ActionResult<OrderDto>> GetByOrderNumber(string orderNumber) {
        _logger.LogInformation($"Get Order for #{orderNumber}, request received.");
        var response = new ResponseBody<OrderDto>();

        var order = await _orderService.GetByOrderNumber(orderNumber).ConfigureAwait(false);

        response.Data = order;
        response.Message = "Order details retrieved successfully";
        _logger.LogInformation(response.Message);

        return Ok(response);
    }

    [HttpPost("place-order")]
    public async Task<IActionResult> PlaceOrder([FromBody] OrderAddDto orderDetails) {
        _logger.LogInformation($"Place order request received for Order: {CamelCaseJsonSerializer.Serialize(orderDetails)}.");
        var response = new ResponseBody<string>();

        var status = await _orderService.PlaceOrderAsync(orderDetails).ConfigureAwait(false);

        response.Message = status;
        _logger.LogInformation(response.Message);

        return Ok(response);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateOrder([FromBody] OrderDto orderDetails) {
        _logger.LogInformation($"Update request received for Order: {CamelCaseJsonSerializer.Serialize(orderDetails)}.");
        var response = new ResponseBody<string>();

        var status = await _orderService.UpdateOrderAsync(orderDetails).ConfigureAwait(false);

        response.Message = status;
        _logger.LogInformation(response.Message);

        return Ok(response);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteOrder(long id) {
        _logger.LogInformation($"Delete request received for Order ID: {id}.");
        var response = new ResponseBody<string>();

        var status = await _orderService.DeleteOrderAsync(id).ConfigureAwait(false);

        response.Message = status;
        _logger.LogInformation(response.Message);

        return Ok(response);
    }

    [HttpGet("search")]
    public async Task<ActionResult<ResponseBody<OrderSearchDto>>> GetProductsForKeyword(string? keyword = null,
        int skip = 0,
        int take = 10,
        string? orderBy = null,
        long customerId = 0) {
        _logger.LogInformation("Search Order request received.");
        var response = new ResponseBody<OrderSearchDto>();

        var result = await _orderService
            .SearchOrdersAsync(keyword, skip, take, orderBy, customerId)
            .ConfigureAwait(false);

        response.Data = result;
        response.Message = "Order details retrieved successfully";

        _logger.LogInformation(response.Message);

        return Ok(response);
    }
}