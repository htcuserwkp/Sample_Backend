namespace Sample.Business.Dtos.OrderDtos;

public class OrderAddDto
{
    public long CustomerId { get; set; }
    public IEnumerable<OrderItemAddDto> OrderItems { get; set; } = Enumerable.Empty<OrderItemAddDto>();
}