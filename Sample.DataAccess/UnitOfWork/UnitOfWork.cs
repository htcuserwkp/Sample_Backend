using Sample.DataAccess.BaseUnitOfWork;
using Sample.DataAccess.Entities;
using Sample.DataAccess.GenericRepository;

namespace Sample.DataAccess.UnitOfWork;

public class UnitOfWork : UnitOfWorkBase, IUnitOfWork
{
    private readonly SampleAppDbContext _context;

    private IGenericRepository<Food>? _foodRepo;
    private IGenericRepository<Order>? _orderRepo;

    public UnitOfWork(SampleAppDbContext context) : base(context)
    {
        _context = context;
    }

    public IGenericRepository<Food> FoodRepo
    {
        get { return _foodRepo ??= new GenericRepository<Food>(_context); }
    }

    public IGenericRepository<Order> OrderRepo
    {
        get { return _orderRepo ??= new GenericRepository<Order>(_context); }
    }
}