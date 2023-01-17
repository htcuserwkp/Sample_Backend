using Sample.DataAccess;

namespace Sample.API;

public static class ServiceExtensions
{
    public static ServiceProvider ServiceProvider { get; private set; } = null!;

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddDbContext<SampleAppDbContext>();

        ServiceProvider = services.BuildServiceProvider(true);
    }
}