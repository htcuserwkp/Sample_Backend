namespace Sample.DataAccess.Entities;

public class BaseEntity
{
    public long Id { get; set; }
    public bool IsDeleted { get; set; }
}