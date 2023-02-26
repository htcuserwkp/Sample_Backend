using Sample.Business.Dtos.OrderDtos;

namespace Sample.Business.Services.OrderBusinessLogic;

public interface IOrderService {

    Task<OrderDto> GetByIdAsync(long id);
    Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
    Task<IEnumerable<OrderDto>> GetByCustomerAsync(long customerId);
    Task<string> PlaceOrderAsync(OrderAddDto orderDetails);
    Task<OrderDto> GetByOrderNumber(string orderNumber);
    Task<string> UpdateOrderAsync(OrderDto orderDetails);
    Task<string> DeleteOrderAsync(long id);
    Task<OrderSearchDto> SearchOrdersAsync(string? keyword = null,
                                            int skip = 0,
                                            int take = 10,
                                            string? orderBy = null,
                                            long customerId = 0);
}