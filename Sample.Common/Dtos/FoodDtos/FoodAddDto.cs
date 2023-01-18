using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Common.Dtos.FoodDtos;

public class FoodAddDto
{
    public required string Name { get; set; } = string.Empty;
    public required string Description { get; set; } = string.Empty;
    public required int Quantity { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public required decimal Price { get; set; }
    public long CategoryId { get; set; }
}