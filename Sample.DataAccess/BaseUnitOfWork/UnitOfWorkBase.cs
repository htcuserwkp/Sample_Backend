using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Sample.DataAccess.Entities;

namespace Sample.DataAccess.BaseUnitOfWork;

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
        await SetModificationInfo();
        await _context.SaveChangesAsync();
    }

    private async Task SetModificationInfo()
    {
        var entities = _context.ChangeTracker.Entries().Where(e => e is
        {
            Entity: BaseEntity, 
            State: EntityState.Added or EntityState.Modified
        });

        string email;
        try
        {
            //var authorizationHeader = _httpContextAccessor?.HttpContext?.Request?.Headers["Authorization"];
            //email = await UserToken.GetCurrentUserEmailAsync(authorizationHeader!);
            email = "defaultUser@sample.com"; //TODO:
        }
        catch (Exception)
        {
            email = "defaultUser@sample.com";
        }

        foreach (var entity in entities)
        {
            if (entity.State == EntityState.Added)
            {
                ((BaseEntity)entity.Entity).CreatedBy = email.ToLower().Trim();
            }
            ((BaseEntity)entity.Entity).ModifiedBy = email.ToLower().Trim();
            ((BaseEntity)entity.Entity).ModifiedOn = DateTime.UtcNow;
        }
    }
}
