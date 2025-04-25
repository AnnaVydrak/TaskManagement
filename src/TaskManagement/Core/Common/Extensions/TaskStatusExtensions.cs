using TaskManagement.Core.Dtos;

namespace TaskManagement.Core.Common.Extensions;

public static class TaskStatusExtensions
{
    public static bool CanTransitionTo(this TaskItemStatus current, TaskItemStatus next) =>
        (current == TaskItemStatus.NotStarted && next == TaskItemStatus.InProgress) ||
        (current == TaskItemStatus.InProgress && next == TaskItemStatus.Completed);
}