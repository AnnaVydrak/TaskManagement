using TaskManagement.Core.Dtos;

namespace TaskManagement.Infrastructure.Entities;

public class TaskItemEntity
{
    public int? Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public TaskItemStatus Status { get; set; } = TaskItemStatus.NotStarted;
    
}