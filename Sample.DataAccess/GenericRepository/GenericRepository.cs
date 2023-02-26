using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sample.Common.Helpers.PredicateBuilder;
using Sample.DataAccess.Entities;

namespace Sample.DataAccess.GenericRepository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int skip = 0, int take = 0)
    {
        IQueryable<TEntity> queryable = _dbSet;

        var predicateBuilder = GetPredicateBuilder(predicate);

        queryable = queryable.Where(predicateBuilder);

        if (orderBy != null) {
            queryable = orderBy(queryable);
        }

        if (take != 0) {
            queryable = queryable.Skip(skip).Take(take);
        }

        return await queryable.ToListAsync();
    }

    public virtual async Task<TEntity> GetByIdAsync(long id)
    {
        return (await _dbSet.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted))!;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.Where(e => !e.IsDeleted).ToListAsync();
    }

    public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        IQueryable<TEntity> queryable = _dbSet;

        var predicateBuilder = GetPredicateBuilder(predicate);

        return (await queryable.FirstOrDefaultAsync(predicateBuilder))!;
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        try
        {
            ValidateEntity(entity);
            await _dbSet.AddAsync(entity);
        }
        catch (DbUpdateException e)
        {
            throw new Exception("Exception occurred while inserting data", e);
        }
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        await Task.Run(() =>
        {
            try
            {
                ValidateEntity(entity);
                _dbSet.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Exception occurred while updating data", e);
            }
        });
    }

    public virtual async Task DeleteAsync(long id)
    {
        var entity = await GetByIdAsync(id);
        await DeleteAsync(entity);
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        ValidateEntity(entity);
        entity.IsDeleted = true;
        await UpdateAsync(entity);
    }

    public virtual async Task<bool> IsActive(long id) {
        var entity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        return entity is not null;
    }

    public virtual async Task<long> GetCountAsync(Expression<Func<TEntity, bool>>? predicate = null) {
        var predicateBuilder = PredicateBuilder.True<TEntity>();
        predicateBuilder = predicateBuilder.And(c => !c.IsDeleted);

        if (predicate != null)
            predicateBuilder = predicateBuilder.And(predicate);

        return await _dbSet.Where(predicateBuilder).CountAsync();
    }

    #region Provate Methods
    private static void ValidateEntity(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentException($"Entity cannot be null");
        }
    }

    private static Expression<Func<TEntity, bool>> GetPredicateBuilder(Expression<Func<TEntity, bool>>? predicate)
    {
        var predicateBuilder = PredicateBuilder.True<TEntity>();
        predicateBuilder = predicateBuilder.And(c => !c.IsDeleted);

        if (predicate != null)
        {
            predicateBuilder = predicateBuilder.And(predicate);
        }

        return predicateBuilder;
    }
    #endregion
}