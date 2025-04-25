using TaskManagement.Core.Dtos;

namespace TaskManagement.Core.Services;

public interface ITaskService
{
    Task<List<TaskItemDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<TaskItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<TaskItemDto> AddAsync(TaskItemCreateDto taskDto, CancellationToken cancellationToken);
    Task<TaskItemDto?> UpdateStatusAsync(int id, TaskItemStatus newStatusDto, CancellationToken cancellationToken);
}