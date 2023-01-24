using Microsoft.Extensions.Logging;
using Sample.Common.Dtos.OrderDtos;
using Sample.Common.Helpers.Exceptions;
using Sample.DataAccess.Entities;
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
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<OrderDto>> GetByCustomerAsync(long customerId)
    {
        throw new NotImplementedException();
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
                OrderItems = await GetOrderItems(foods, orderDetails.OrderItems).ConfigureAwait(false)
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

            await _unitOfWork.SaveChangesAsync();
            status = "Order placed successfully";
            _logger.LogDebug(status);
            _logger.LogDebug("Stocks Reduced successfully for Pre-prepared Food");
        }
        catch (Exception e)
        {
            status = "Order placement failed";
            _logger.LogError($"{status} Error: {e}");
            throw;
        }
        return status;
    }

    private static async Task<ICollection<OrderItem>> GetOrderItems(IEnumerable<Food> foods, IEnumerable<OrderItemAddDto> orderDetailsOrderItems)
    {
        return foods.Select(item => new OrderItem()
        {
            FoodId = item.Id,
            FoodName = item.Name,
            Price = item.Price,
            Quantity = (orderDetailsOrderItems.FirstOrDefault(i => i.FoodId == item.Id)!).Quantity
        }) .ToList();
    }
}