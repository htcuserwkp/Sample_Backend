using Sample.Business.Dtos.OrderDtos;

namespace Sample.Business.Services.OrderBusinessLogic;

public interface IOrderService
{
    Task<OrderDto> GetByIdAsync(long id);
    Task<IEnumerable<OrderDto>> GetAllAsync();
    Task<IEnumerable<OrderDto>> GetByCustomerAsync(long customerId);
    Task<string> PlaceOrderAsync(OrderAddDto orderDetails);
    Task<OrderDto> GetByOrderNumber(string orderNumber);
}