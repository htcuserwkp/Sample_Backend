﻿using Sample.DataAccess.BaseUnitOfWork;
using Sample.DataAccess.Entities;
using Sample.DataAccess.GenericRepository;

namespace Sample.DataAccess.UnitOfWork;

public class UnitOfWork : UnitOfWorkBase, IUnitOfWork
{
    private readonly SampleAppDbContext _context;

    private IGenericRepository<Food> _foodRepo = null!;

    public UnitOfWork(SampleAppDbContext context) : base(context)
    {
        _context = context;
    }

    public IGenericRepository<Food> FoodRepo
    {
        get
        {
            if (_foodRepo == null)
            {
                _foodRepo = new GenericRepository<Food>(_context);
            }
            return _foodRepo;
        }
    }
}