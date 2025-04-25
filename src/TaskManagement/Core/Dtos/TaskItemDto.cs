
namespace TaskManagement.Core.Dtos;

public record TaskItemDto
{
    public int Id { get; init; }
    
    public string Name { get; init; } = null!;
    
    public string Description { get; init; } = null!;
    
    public TaskItemStatus Status { get; init; } = TaskItemStatus.NotStarted;
}