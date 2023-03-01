using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Business.Dtos.FoodDtos;

public class FoodAddDto
{
    [Required(ErrorMessage = $"{nameof(Name)} is required")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = $"{nameof(Description)} is required")]
    public string Description { get; set; } = string.Empty;

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    public long CategoryId { get; set; }
}