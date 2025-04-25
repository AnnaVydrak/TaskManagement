using TaskManagement.Messaging.Events;

namespace TaskManagement.Messaging;

public interface ITaskEventProducer
{
    Task PublishTaskCompletedAsync(TaskCompletedEvent @event, CancellationToken cancellationToken);
}