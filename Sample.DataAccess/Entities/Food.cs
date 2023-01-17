using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.DataAccess.Entities;

public class Food : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
}