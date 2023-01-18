namespace Sample.Common.Dtos.OrderDtos;

public class OrderAddDto
{
    public long CustomerId { get; set; }
    public IEnumerable<long> FoodIds { get; set; } = Enumerable.Empty<long>();
}