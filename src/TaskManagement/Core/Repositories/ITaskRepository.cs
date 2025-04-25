using TaskManagement.Core.Dtos;

namespace TaskManagement.Core.Repositories;

public interface ITaskRepository
{
    Task<List<TaskItemDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<TaskItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<TaskItemDto> AddAsync(TaskItemCreateDto task, CancellationToken cancellationToken);
    Task<TaskItemDto?> UpdateStatusAsync(int id, TaskItemStatus newStatus, CancellationToken cancellationToken);
}
