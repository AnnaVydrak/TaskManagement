using TaskManagement.Core.Common.Exceptions;
using TaskManagement.Core.Common.Extensions;
using TaskManagement.Core.Common.Providers;
using TaskManagement.Core.Dtos;
using TaskManagement.Core.Repositories;
using TaskManagement.Messaging;
using TaskManagement.Messaging.Events;

namespace TaskManagement.Core.Services;

public class TaskService(
    ITaskRepository taskRepository, 
    ITaskEventProducer taskEventProducer,
    IDateTimeProvider  dateTimeProvider) : ITaskService
{
    public Task<List<TaskItemDto>> GetAllAsync(CancellationToken cancellationToken)
        => taskRepository.GetAllAsync(cancellationToken);

    public Task<TaskItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => taskRepository.GetByIdAsync(id, cancellationToken);

    public async Task<TaskItemDto> AddAsync(TaskItemCreateDto taskCreated, CancellationToken cancellationToken)
    {
        var newItem = await taskRepository.AddAsync(taskCreated, cancellationToken);
        await TryPublishTaskCompletedAsync(newItem!, cancellationToken);

        return newItem;
    }

    public async Task<TaskItemDto?> UpdateStatusAsync(int id, TaskItemStatus newStatus, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetByIdAsync(id, cancellationToken);
        if (task == null)
            throw new ArgumentException("Task not found.");

        if (!task.Status.CanTransitionTo(newStatus))
            throw new InvalidStatusTransitionException(task.Status, newStatus);
        
        var updatedTask = await taskRepository.UpdateStatusAsync(id, newStatus, cancellationToken);
        await TryPublishTaskCompletedAsync(updatedTask!, cancellationToken);

        return updatedTask;
    }

    private async Task TryPublishTaskCompletedAsync(TaskItemDto taskItem, CancellationToken cancellationToken)
    {
        //TODO move other service If more logic
        if (taskItem.Status == TaskItemStatus.Completed)
        {
            //TODO think about outbox pattern
            await taskEventProducer.PublishTaskCompletedAsync(new TaskCompletedEvent
            {
                TaskId = taskItem.Id,
                Name = taskItem.Name,
                CompletedAt = dateTimeProvider.UtcNow
            }, cancellationToken);
        }
    }
}