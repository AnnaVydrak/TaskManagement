using TaskManagement.Core.Common.Providers;
using TaskManagement.Core.Repositories;
using TaskManagement.Core.Services;
using TaskManagement.Infrastructure.PostgreSql.Repositories;

namespace TaskManagement.Core.Extensions;

public static class ServiceCollectionExtension
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<ITaskService, TaskService>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
    }
}