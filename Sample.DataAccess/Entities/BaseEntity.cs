namespace Sample.DataAccess.Entities;

public class BaseEntity
{
    public required long Id { get; set; }
    public required bool IsDeleted { get; set; }
}