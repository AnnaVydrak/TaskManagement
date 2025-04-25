using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskManagement.Infrastructure.PostgreSql;

public class AppDbContextFactory: IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var configBuilder = new ConfigurationBuilder();
        var envConfigBuilder = EnvironmentVariablesExtensions.AddEnvironmentVariables(
            configBuilder
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings." + (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")) + ".json", true, true));
        var configuration = envConfigBuilder
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        optionsBuilder.EnableDetailedErrors();

        return new AppDbContext(optionsBuilder.Options);
    }
}