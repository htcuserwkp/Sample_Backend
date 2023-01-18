namespace Sample.DataAccess.Entities;

public class Category : BaseEntity
{
    public required string Name { get; set; } = string.Empty;
    public required string Description { get; set; } = string.Empty;
    public virtual ICollection<Food>? Foods { get; set; }
}