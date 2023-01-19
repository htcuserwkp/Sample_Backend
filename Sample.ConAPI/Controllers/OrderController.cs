﻿using Microsoft.AspNetCore.Mvc;
using Sample.Business.OrderBusinessLogic;
using Sample.Common.Dtos.OrderDtos;

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
    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
    {
        var orders = await _orderService.GetAllAsync().ConfigureAwait(false);

        return orders.Any() ? Ok(orders) : NotFound();
    }

    [HttpGet("GetByCustomer")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetByCustomer(long customerId)
    {
        var orders = await _orderService.GetByCustomerAsync(customerId).ConfigureAwait(false);

        return orders.Any() ? Ok(orders) : NotFound();
    }

    [HttpGet("GetById")]
    public async Task<ActionResult<OrderDto>> GetById(long id)
    {
        var order = await _orderService.GetByIdAsync(id).ConfigureAwait(false);

        return Ok(order);
    }

    [HttpPost("PlaceOrder")]
    public async Task<IActionResult> PlaceOrder([FromBody] OrderAddDto orderDetails)
    {
        var status = await _orderService.PlaceOrderAsync(orderDetails).ConfigureAwait(false);

        return Ok(status);
    }
}