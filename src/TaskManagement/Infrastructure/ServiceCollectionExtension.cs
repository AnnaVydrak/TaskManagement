using Microsoft.EntityFrameworkCore;
using TaskManagement.Infrastructure.PostgreSql;

namespace TaskManagement.Infrastructure;

public static class ServiceCollectionExtension
{
    public static void ConfigureDatabase(
        this IServiceCollection services, 
        ConfigurationManager configuration)
    {
        services.AddDbContextPool<AppDbContext>(
            options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );
        
        services.AddDbContext<DbContext, AppDbContext>();
    }
}