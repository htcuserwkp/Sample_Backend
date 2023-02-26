namespace Sample.Business.Dtos.OrderDtos;

public class OrderItemDto : OrderItemAddDto
{
    public string FoodName { get; set; } = string.Empty;
    public decimal Price { get; set; }
}