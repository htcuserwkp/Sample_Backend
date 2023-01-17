namespace Sample.DataAccess.BaseUnitOfWork;

public interface IUnitOfWorkBase
{
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task SaveChangesAsync();
}
