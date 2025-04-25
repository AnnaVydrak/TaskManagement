using MassTransit;
using TaskManagement.Core.Common.Constants;
using TaskManagement.Messaging.Azure;
using TaskManagement.Messaging.RabbitMq;

namespace TaskManagement.Messaging;

public static class MessageBusConfiguration
{
    public static void ConfigureMessageBus( 
        this IServiceCollection services, 
        ConfigurationManager configuration)
    {
        var useBus = configuration["UseMessageBus"];

        switch (useBus)
        {
            case MessageBusTypes.Azure:
                ConfigureAzureServiceBus(services, configuration);
                break;
            case MessageBusTypes.RabbitMQ:
                ConfigureRabbitMq(services, configuration);
                break;
            default:
                throw new Exception($"Invalid message bus configuration. Found: {useBus}");
        }
    }
    
    private static void ConfigureAzureServiceBus(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<TaskCompletedEventConsumer>();

            x.UsingAzureServiceBus((context, cfg) =>
            {
                cfg.Host(configuration["AzureServiceBus:ConnectionString"]);

                cfg.ReceiveEndpoint("task-events", e =>
                {
                    e.ConfigureConsumer<TaskCompletedEventConsumer>(context);
                });
            });
        });

        services.AddScoped<ITaskEventProducer, AzureEventProducer>();
    }
    
    private static void ConfigureRabbitMq(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<TaskCompletedEventConsumer>();
        
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMQ:Host"]);
                
                cfg.ReceiveEndpoint("task-events", e =>
                {
                    e.ConfigureConsumer<TaskCompletedEventConsumer>(context);
                });
            });
        });

        services.AddScoped<ITaskEventProducer, RabbitEventProducer>();
    }
}