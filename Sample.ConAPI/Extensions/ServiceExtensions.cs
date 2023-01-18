using Sample.Business.FoodBusinessLogic;
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

        ServiceProvider = services.BuildServiceProvider(true);
    }
}