using Microsoft.EntityFrameworkCore;
using TaskManagement.Core.Common.Exceptions;
using TaskManagement.Core.Extensions;
using TaskManagement.Messaging;
using TaskManagement.Infrastructure;
using TaskManagement.Infrastructure.PostgreSql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureMessageBus(builder.Configuration);
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

ExceptionHandlingConfiguration.ConfigureExceptionHandling(app);

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManagement API V1");
});

app.MapControllers();

try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    await services.GetRequiredService<AppDbContext>().Database.MigrateAsync();
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "An error occurred while initializing the application.");
    return;
}

app.Run();