using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.DataAccess.Entities;

public class Food : BaseEntity
{
    [MaxLength(128), Required]
    public required string Name { get; set; } = string.Empty;

    [MaxLength(512), Required]
    public required string Description { get; set; } = string.Empty;

    public required int Quantity { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public required decimal Price { get; set; }
    public bool IsFreshlyPrepared { get; set; } = false;

    public long CategoryId { get; set; }
    public virtual Category Category { get; set; } = null!;
}