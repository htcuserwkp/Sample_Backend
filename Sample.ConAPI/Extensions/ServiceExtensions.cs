using Sample.Business.MappingProfiles;
using Sample.Business.Services.CategoryBusinessLogic;
using Sample.Business.Services.CustomerBusinessLogic;
using Sample.Business.Services.FoodBusinessLogic;
using Sample.Business.Services.OrderBusinessLogic;
using Sample.DataAccess;
using Sample.DataAccess.UnitOfWork;

namespace Sample.API.Extensions;

public static class ServiceExtensions {
    public static ServiceProvider ServiceProvider { get; private set; } = null!;

    public static void RegisterServices(this IServiceCollection services) {
        services.AddDbContext<SampleAppDbContext>();

        services.AddAutoMapper(typeof(OrderMappingProfile),
            typeof(FoodMappingProfile),
            typeof(CategoryMappingProfile),
            typeof(CustomerMappingProfile));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IFoodService, FoodService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICustomerService, CustomerService>();

        ServiceProvider = services.BuildServiceProvider(true);
    }
}