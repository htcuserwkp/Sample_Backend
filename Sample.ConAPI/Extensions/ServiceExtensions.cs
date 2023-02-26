using Sample.Business.MappingProfiles;
using Sample.Business.Services.CategoryBusinessLogic;
using Sample.Business.Services.FoodBusinessLogic;
using Sample.Business.Services.OrderBusinessLogic;
using Sample.DataAccess;
using Sample.DataAccess.UnitOfWork;

namespace Sample.API.Extensions;

public static class ServiceExtensions
{
    public static ServiceProvider ServiceProvider { get; private set; } = null!;

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddDbContext<SampleAppDbContext>();

        services.AddAutoMapper(typeof(OrderMappingProfile),
                                typeof(FoodMappingProfile),
                                typeof(CategoryMappingProfile));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IFoodService, FoodService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ICategoryService, CategoryService>();

        ServiceProvider = services.BuildServiceProvider(true);
    }
}