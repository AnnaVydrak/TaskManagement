using TaskManagement.Core.Dtos;

namespace TaskManagement.Core.Common.Exceptions;

public class InvalidStatusTransitionException : Exception
{
    public InvalidStatusTransitionException(
        TaskItemStatus current,
        TaskItemStatus attempted)
        : base($"Cannot change status from {current} to {attempted}.")
    {
    }
}