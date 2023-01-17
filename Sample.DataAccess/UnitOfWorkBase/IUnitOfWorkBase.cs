namespace Sample.DataAccess.UnitOfWorkBase;

public interface IUnitOfWorkBase
{
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task SaveChangesAsync();
}
