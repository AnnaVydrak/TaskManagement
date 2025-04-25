using MassTransit;
using TaskManagement.Messaging.Events;

namespace TaskManagement.Messaging.Azure;

public class AzureEventProducer(IPublishEndpoint publishEndpoint) : ITaskEventProducer
{
    public Task PublishTaskCompletedAsync(TaskCompletedEvent @event, CancellationToken cancellationToken)
    {
        return publishEndpoint.Publish(@event, cancellationToken);
    }
}