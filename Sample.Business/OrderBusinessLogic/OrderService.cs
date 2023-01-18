using Sample.Common.Dtos.OrderDtos;

namespace Sample.Business.OrderBusinessLogic;

public class OrderService : IOrderService
{
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
        throw new NotImplementedException();
    }
}