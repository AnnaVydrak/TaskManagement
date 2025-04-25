using MassTransit;
using TaskManagement.Messaging.Events;

namespace TaskManagement.Messaging;

public class TaskCompletedEventConsumer(ILogger<TaskCompletedEventConsumer> logger) : IConsumer<TaskCompletedEvent>
{
    public Task Consume(ConsumeContext<TaskCompletedEvent> context)
    {
        var message = context.Message;
        logger.LogInformation("Consumed TaskCompletedEvent | TaskId: {TaskId}, Title: {Title}",
            message.TaskId, message.Name);
        return Task.CompletedTask;
    }
}