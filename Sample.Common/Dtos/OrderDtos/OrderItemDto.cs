using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Common.Dtos.OrderDtos;

public class OrderItemDto : OrderItemAddDto
{
    public string FoodName { get; set; } = string.Empty;
    public decimal Price { get; set; }
}