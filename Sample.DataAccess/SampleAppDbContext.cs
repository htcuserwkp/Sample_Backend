
using Microsoft.EntityFrameworkCore;
using Sample.Common;
using Sample.DataAccess.Entities;

namespace Sample.DataAccess;

public class SampleAppDbContext : DbContext
{
    public SampleAppDbContext(DbContextOptions<SampleAppDbContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(AppConfig.ConnectionString);
        }
    }

    public virtual DbSet<Food> Foods { get; set; } = null!;
    public virtual DbSet<Category> Categories { get; set; } = null!;
    public virtual DbSet<Customer> Customers { get; set; } = null!;
    public virtual DbSet<Order> Orders { get; set; } = null!;
    public virtual DbSet<Log> Logs { get; set; } = null!;
    public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
}