using Sample.DataAccess.BaseUnitOfWork;
using Sample.DataAccess.Entities;
using Sample.DataAccess.GenericRepository;

namespace Sample.DataAccess.UnitOfWork;

public interface IUnitOfWork : IUnitOfWorkBase
{
    IGenericRepository<Food> FoodRepo { get; }
}