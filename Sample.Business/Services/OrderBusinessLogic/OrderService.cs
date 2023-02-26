using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sample.Business.Dtos;
using Sample.Business.Dtos.CustomerDtos;
using Sample.Business.Dtos.OrderDtos;
using Sample.Common.Helpers.Exceptions;
using Sample.Common.Helpers.PredicateBuilder;
using Sample.DataAccess.Entities;
using Sample.DataAccess.UnitOfWork;

namespace Sample.Business.Services.OrderBusinessLogic;

public class OrderService : IOrderService {
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<OrderService> _logger;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork unitOfWork, ILogger<OrderService> logger, IMapper mapper) {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<OrderDto> GetByIdAsync(long id) {
        var order = await _unitOfWork.OrderRepo.GetByIdAsync(id);
        if (order is null)
            throw new CustomException {
                CustomMessage = "No Order Found",
                HttpStatusCode = HttpStatusCode.NotFound
            };

        var customerEmail = (await _unitOfWork.CustomerRepo.GetByIdAsync(order.CustomerId)).Email;

        return GetOrderDetails(order, customerEmail);
    }

    public async Task<OrderDto> GetByOrderNumber(string orderNumber) {
        var order = await _unitOfWork.OrderRepo.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber.Trim());
        if (order is null)
            throw new CustomException {
                CustomMessage = "No Order Found",
                HttpStatusCode = HttpStatusCode.NotFound
            };

        var customerEmail = (await _unitOfWork.CustomerRepo.GetByIdAsync(order.CustomerId)).Email;

        return GetOrderDetails(order, customerEmail);
    }

    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync() {
        var orders = (await _unitOfWork.OrderRepo
            .GetAllAsync()
            .ConfigureAwait(false))
            .ToList();

        if (orders is null || !orders.Any())
            throw new CustomException {
                CustomMessage = "No Orders Found",
                HttpStatusCode = HttpStatusCode.NotFound
            };

        var emailDetails = (await _unitOfWork.CustomerRepo
            .GetAsync(c => orders.Select(o => o.CustomerId).Contains(c.Id)))
            .Select(c => new EmailDto() {
                Id = c.Id,
                Email = c.Email
            });

        return GetOrderListResponse(orders, emailDetails);
    }

    public async Task<IEnumerable<OrderDto>> GetByCustomerAsync(long customerId) {
        var orders = (await _unitOfWork.OrderRepo
            .GetAsync(o => o.CustomerId == customerId)
            .ConfigureAwait(false))
            .ToList();

        if (orders is null || !orders.Any())
            throw new CustomException {
                CustomMessage = "No Orders Found",
                HttpStatusCode = HttpStatusCode.NotFound
            };

        var emailDetails = new List<EmailDto>
        {
            new()
            {
                Id = customerId,
                Email = (await _unitOfWork.CustomerRepo.GetByIdAsync(customerId)).Email
            }
        };

        return GetOrderListResponse(orders, emailDetails);
    }


    public async Task<string> PlaceOrderAsync(OrderAddDto orderDetails) {
        string status;
        try {
            //Get order items(foods)
            var foods = (await _unitOfWork.FoodRepo
                .GetAsync((f => orderDetails.OrderItems.Select(i => i.FoodId).Contains(f.Id)))
                .ConfigureAwait(false))
                .ToList();

            if (foods is null || !foods.Any()) {
                throw new CustomException {
                    CustomMessage = "Your have not selected any item",
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            //Create Order
            Order order = new() {
                OrderNumber = "100" + new Random().Next(100000000, 999999999).ToString(),
                CustomerId = orderDetails.CustomerId,
                OrderItems = GetOrderItems(foods, orderDetails.OrderItems)
            };

            //Calculate Order Total
            foreach (var item in order.OrderItems.Select(i => new { i.Price, i.Quantity })) {
                order.SubTotal += item.Quantity * item.Price;
            }
            //TODO: Discount and Service charge
            order.Total = order.SubTotal - order.Discount + order.ServiceCharge;

            //Add the order
            await _unitOfWork.OrderRepo.AddAsync(order);

            //reduce food quantities accordingly //TODO: move to foodService
            foreach (var food in foods.Where(f => !f.IsFreshlyPrepared)) {
                food.Quantity -= (order.OrderItems.FirstOrDefault(s => s.FoodId == food.Id)!.Quantity);
                await _unitOfWork.FoodRepo.UpdateAsync(food);
            }

            await _unitOfWork.SaveChangesAsync();
            status = "Order placed successfully";
            _logger.LogDebug(status);

            if (foods.Any(f => !f.IsFreshlyPrepared))
                _logger.LogDebug("Stocks Reduced successfully for Pre-prepared Foods");
        }
        catch (Exception e) {
            status = "Order placement failed";
            _logger.LogError($"{status} Error: {e}");
            throw;
        }
        return status;
    }


    public async Task<string> UpdateOrderAsync(OrderDto orderDetails) {
        string status;
        try {
            //TODO: validate details

            var order = await _unitOfWork.OrderRepo.GetByIdAsync(orderDetails.Id);

            if (order is null) {
                throw new CustomException {
                    CustomMessage = "Order Not Found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            // Map order details to order entity
            _mapper.Map(orderDetails, order);

            await _unitOfWork.OrderRepo.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            status = "Order updated successfully";
            _logger.LogDebug(status);
        }
        catch (Exception e) {
            status = "Order failed to update";
            _logger.LogError($"{status} Error: {e}");
            throw;
        }

        return status;
    }


    public async Task<string> DeleteOrderAsync(long id) {
        string status;
        try {
            //check availability
            if (!await _unitOfWork.OrderRepo.IsActive(id)) {
                throw new CustomException {
                    CustomMessage = "Order not found!",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }
            await _unitOfWork.OrderRepo.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            status = "Order deleted successfully";
            _logger.LogDebug(status);
        }
        catch (Exception e) {
            status = "Order deletion failed";
            _logger.LogError($"{status} Error: {e}");
            throw;
        }
        return status;
    }

    public async Task<OrderSearchDto> SearchOrdersAsync(string? keyword = null, int skip = 0, int take = 10, string? orderBy = null, long customerId = 0) {
        var orderPredicate = PredicateBuilder.True<Order>();

        //filter by keyword
        if (!string.IsNullOrEmpty(keyword)) {
            orderPredicate = SearchExpressionFilter(orderPredicate, keyword.Trim());
        }

        //filter by customer
        if (customerId > 0) {
            orderPredicate = orderPredicate.And(p => p.CustomerId == customerId);
        }

        //TODO: order by

        var orders = await _unitOfWork.OrderRepo.GetAsync(predicate: orderPredicate, skip: skip, take: take, orderBy: null);
        return new OrderSearchDto {
            Orders = _mapper.Map<IEnumerable<OrderDto>>(orders),
            Page = new PaginationDto() {
                CurrentCount = orders.Count(),
                TotalCount = await _unitOfWork.OrderRepo.GetCountAsync(orderPredicate)
            }
        };
    }

    #region Private Methods
    private static Expression<Func<Order, bool>> SearchExpressionFilter(Expression<Func<Order, bool>> orderPredicate, string keyword) {
        return orderPredicate.And(c => (c.OrderNumber.Contains(keyword) ||
                                       c.Id.ToString().Contains(keyword)));
    }

    private IEnumerable<OrderDto> GetOrderListResponse(IEnumerable<Order> orders, IEnumerable<EmailDto> emailDetails) {
        var customerEmails = emailDetails.ToDictionary(e => e.Id, e => e.Email);

        return orders
            .Select(order => GetOrderDetails(order, customerEmails[order.CustomerId]))
            .ToList();
    }

    private OrderDto GetOrderDetails(Order order, string customerEmail) {
        return _mapper.Map<OrderDto>(order, opt => opt.Items["CustomerEmail"] = customerEmail);
    }

    private static ICollection<OrderItem> GetOrderItems(IEnumerable<Food> foods, IEnumerable<OrderItemAddDto> orderDetailsOrderItems) {
        return foods.Select(food => new OrderItem {
            FoodId = food.Id,
            FoodName = food.Name,
            Price = food.Price,
            Quantity = orderDetailsOrderItems.FirstOrDefault(i => i.FoodId == food.Id)?.Quantity ?? 0
        }).ToList();
    }
    #endregion
}