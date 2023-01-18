namespace Sample.DataAccess.Entities;

public class Customer : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public virtual ICollection<Order>? Orders { get; set; }
}