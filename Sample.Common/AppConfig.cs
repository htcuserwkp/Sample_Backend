using Microsoft.Extensions.Configuration;

namespace Sample.Common
{
    public static class AppConfig
    {
        public static IConfiguration Configuration { get; set; } = null!;
        public static string ConnectionString => Configuration.GetConnectionString("DefaultConnection") 
                                                 ?? throw new InvalidOperationException();
    }
}
