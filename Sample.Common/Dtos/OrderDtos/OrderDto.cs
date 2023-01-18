using System.ComponentModel.DataAnnotations.Schema;
using Sample.Common.Dtos.FoodDtos;

namespace Sample.Common.Dtos.OrderDtos;

public class OrderDto
{
    public string OrderNumber { get; set; } = null!;

    public DateTime OrderPlaced { get; set; }
    
    public decimal SubTotal { get; set; }
    
    public decimal Discount { get; set; } = 0;
    
    public decimal ServiceCharge { get; set; } = 0;
    
    public decimal Total { get; set; }

    public IEnumerable<FoodDto> Foods { get; set; } = Enumerable.Empty<FoodDto>();

    public long CustomerId { get; set; }

    public long CustomerEmail { get; set; }
}