using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Sample.DataAccess.UnitOfWorkBase;

public class UnitOfWorkBase : IUnitOfWorkBase
{
    private readonly DbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWorkBase(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction == null)
            return;

        try
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }
        catch (Exception)
        {
            await _transaction.RollbackAsync();
            throw;
        }
        finally
        {
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction == null)
            return;

        try
        {
            await _transaction.CommitAsync();
        }
        finally
        {
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
