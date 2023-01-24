using Microsoft.Extensions.Logging;
using Sample.Common.Dtos.OrderDtos;
using Sample.Common.Helpers.Exceptions;
using Sample.DataAccess.Entities;
using Sample.DataAccess.Migrations;
using Sample.DataAccess.UnitOfWork;
using System.Net;

namespace Sample.Business.OrderBusinessLogic;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<OrderService> _logger;
    public OrderService(IUnitOfWork unitOfWork, ILogger<OrderService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<OrderDto> GetByIdAsync(long id)
    {
        var order = await _unitOfWork.OrderRepo.GetByIdAsync(id);
        if (order == null)
            throw new CustomException
            {
                CustomMessage = "No Order Found",
                HttpStatusCode = HttpStatusCode.NotFound
            };

        var customerEmail = (await _unitOfWork.CustomerRepo.GetByIdAsync(order.CustomerId)).Email;

        return GetOrderDetails(order, customerEmail);
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<OrderDto>> GetByCustomerAsync(long customerId)
    {
        var orders = await _unitOfWork.OrderRepo.GetAsync(o => o.CustomerId == customerId).ConfigureAwait(false);
        if (orders == null)
            throw new CustomException
            {
                CustomMessage = "No Orders Found",
                HttpStatusCode = HttpStatusCode.NotFound
            };

        var customerEmail = (await _unitOfWork.CustomerRepo.GetByIdAsync(customerId)).Email;

        return GetOrderListResponse(orders, customerEmail);
    }

    public async Task<string> PlaceOrderAsync(OrderAddDto orderDetails)
    {
        string status;
        try
        {
            //Get order items(foods)
            var foods = (await _unitOfWork.FoodRepo
                .GetAsync((f => orderDetails.OrderItems.Select(i => i.FoodId).Contains(f.Id)))
                .ConfigureAwait(false))
                .ToList();

            if (foods == null || !foods.Any())
            {
                throw new CustomException
                {
                    CustomMessage = "Your have not selected any item",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            //Create Order
            Order order = new()
            {
                OrderNumber = "100" + new Random().Next(100000000, 999999999).ToString(),
                CustomerId = orderDetails.CustomerId,
                OrderItems = GetOrderItems(foods, orderDetails.OrderItems)
            };
            
            //Calculate Order Total
            foreach (var item in order.OrderItems.Select(i => new { i.Price, i.Quantity }))
            {
                order.SubTotal += item.Quantity * item.Price;
            }
            //TODO: Discount and Service charge
            order.Total = order.SubTotal - order.Discount + order.ServiceCharge;

            //Add the order
            await _unitOfWork.OrderRepo.AddAsync(order);

            //reduce food quantities accordingly //TODO: move to foodService
            foreach (var food in foods.Where(f => !f.IsFreshlyPrepared))
            {
                food.Quantity -= (order.OrderItems.FirstOrDefault(s => s.FoodId == food.Id)!.Quantity);
                await _unitOfWork.FoodRepo.UpdateAsync(food);
            }

            await _unitOfWork.SaveChangesAsync();
            status = "Order placed successfully";
            _logger.LogDebug(status);

            if (foods.Any(f => !f.IsFreshlyPrepared)) 
                _logger.LogDebug("Stocks Reduced successfully for Pre-prepared Foods");
        }
        catch (Exception e)
        {
            status = "Order placement failed";
            _logger.LogError($"{status} Error: {e}");
            throw;
        }
        return status;
    }


    #region Private Methods
    private static  ICollection<OrderItem> GetOrderItems(IEnumerable<Food> foods, IEnumerable<OrderItemAddDto> orderDetailsOrderItems)
    {
        return foods.Select(item => new OrderItem()
        {
            FoodId = item.Id,
            FoodName = item.Name,
            Price = item.Price,
            Quantity = (orderDetailsOrderItems.FirstOrDefault(i => i.FoodId == item.Id)!).Quantity
        }) .ToList();
    }

    private static OrderDto GetOrderDetails(Order order, string customerEmail)
    {
        return new OrderDto()
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            CustomerId = order.CustomerId,
            CustomerEmail = customerEmail,
            OrderPlaced = order.OrderPlaced,
            SubTotal = order.SubTotal,
            ServiceCharge = order.ServiceCharge,
            Discount = order.Discount,
            Total = order.Total,
            OrderItems = GetOrderItemsList(order.OrderItems)
        };
    }

    private static IEnumerable<OrderItemDto> GetOrderItemsList(IEnumerable<OrderItem> orderOrderItems)
    {
        return orderOrderItems.Select(item => new OrderItemDto()
        {
            Quantity = item.Quantity,
            Price = item.Price,
            FoodId = item.FoodId,
            FoodName = item.FoodName
        }).ToList();
    }

    private static IEnumerable<OrderDto> GetOrderListResponse(IEnumerable<Order> orders, string customerEmail)
    {
        return orders
            .Select(order => GetOrderDetails(order, customerEmail))
            .ToList();
    }
    #endregion
}