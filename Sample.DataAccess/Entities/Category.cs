using System.ComponentModel.DataAnnotations;

namespace Sample.DataAccess.Entities;

public class Category : BaseEntity
{
    [MaxLength(128), Required]
    public required string Name { get; set; } = string.Empty;

    [MaxLength(512), Required]
    public required string Description { get; set; } = string.Empty;

    public virtual ICollection<Food>? Foods { get; set; }
}