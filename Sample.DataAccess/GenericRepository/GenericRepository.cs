using System.Linq.Expressions;
using Sample.DataAccess.Entities;

namespace Sample.DataAccess.GenericRepository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    public Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TEntity>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }
}