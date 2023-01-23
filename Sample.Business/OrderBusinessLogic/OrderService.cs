using Microsoft.Extensions.Logging;
using Sample.Common.Dtos.OrderDtos;
using Sample.DataAccess.Entities;
using Sample.DataAccess.UnitOfWork;

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
        
        var foods = await _unitOfWork.FoodRepo
                .GetAsync(f => orderDetails.FoodIds.Contains(f.Id))
                .ConfigureAwait(false);
            
        throw new NotImplementedException();
    }
}