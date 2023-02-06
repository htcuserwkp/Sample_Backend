using System.ComponentModel.DataAnnotations;

namespace Sample.DataAccess.Entities;

public class Customer : BaseEntity
{
    [MaxLength(128), Required]
    public string Name { get; set; } = null!;

    [MaxLength(128), Required]
    public string Email { get; set; } = null!;

    [MaxLength(20), Required]
    public string Phone { get; set; } = null!;
    public virtual ICollection<Order>? Orders { get; set; }
}