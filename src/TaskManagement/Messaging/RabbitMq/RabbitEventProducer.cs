using MassTransit;
using TaskManagement.Messaging.Events;

namespace TaskManagement.Messaging.RabbitMq;

public class RabbitEventProducer(IPublishEndpoint publishEndpoint) : ITaskEventProducer
{
    public async Task PublishTaskCompletedAsync(TaskCompletedEvent @event, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(@event, cancellationToken);
    }
}