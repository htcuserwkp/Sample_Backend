using Sample.Business.FoodBusinessLogic;
using Sample.Business.OrderBusinessLogic;
using Sample.DataAccess;
using Sample.DataAccess.UnitOfWork;

namespace Sample.API.Extensions;

public static class ServiceExtensions
{
    public static ServiceProvider ServiceProvider { get; private set; } = null!;

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddDbContext<SampleAppDbContext>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IFoodService, FoodService>();
        services.AddScoped<IOrderService, OrderService>();

        ServiceProvider = services.BuildServiceProvider(true);
    }
}