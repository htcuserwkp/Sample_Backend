using System.Linq.Expressions;
using Sample.DataAccess.Entities;

namespace Sample.DataAccess.GenericRepository;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null);

    Task<TEntity> GetByIdAsync(long id);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null);

    Task AddAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(long id);

    Task DeleteAsync(TEntity entity);

    Task<bool> IsActive(long id);
}