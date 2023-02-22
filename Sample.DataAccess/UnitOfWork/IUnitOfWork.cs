using Sample.DataAccess.BaseUnitOfWork;
using Sample.DataAccess.Entities;
using Sample.DataAccess.GenericRepository;

namespace Sample.DataAccess.UnitOfWork;

public interface IUnitOfWork : IUnitOfWorkBase
{
    IGenericRepository<Food> FoodRepo { get; }
    IGenericRepository<Order> OrderRepo { get; }
    IGenericRepository<Customer> CustomerRepo { get; }
    IGenericRepository<Category> CategoryRepo { get; }
}