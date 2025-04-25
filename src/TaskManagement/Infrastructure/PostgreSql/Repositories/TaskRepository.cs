using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Core.Dtos;
using TaskManagement.Core.Repositories;
using TaskManagement.Infrastructure.Entities;

namespace TaskManagement.Infrastructure.PostgreSql.Repositories;

public class TaskRepository(
    IMapper mapper,
    AppDbContext context) : ITaskRepository
{
    #region PaginationSupport

    /// todo: If we have a lot of data, we need to implement pagination
    ///
    /// public async Task<List<TaskItem>> GetAllAsync(int page, int pageSize)
    /// {
    ///    return await _context.Tasks
    ///         .OrderBy(t => t.Id)
    ///         .Skip((page - 1) * pageSize)
    ///        .Take(pageSize)
    ///        .ToListAsync();
    ///  }

    #endregion
    
    public async Task<List<TaskItemDto>> GetAllAsync(CancellationToken cancellationToken) => await context.Tasks
        .AsNoTracking()
        .Select(p => mapper.Map<TaskItemDto>(p))
        .ToListAsync(cancellationToken);

    public async Task<TaskItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var taskItem = await context.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        
        return mapper.Map<TaskItemDto?>(taskItem);
    }

    public async Task<TaskItemDto> AddAsync(TaskItemCreateDto task, CancellationToken cancellationToken)
    {
        var taskEntity = mapper.Map<TaskItemEntity>(task);
        context.Tasks.Add(taskEntity);
        await context.SaveChangesAsync(cancellationToken);
        
        return mapper.Map<TaskItemDto>(taskEntity);
    }

    public async Task<TaskItemDto?> UpdateStatusAsync(int id, TaskItemStatus newStatus, CancellationToken cancellationToken)
    {
        var taskItem = await context.Tasks.FindAsync(id);

        if (taskItem == null)
        {
            return null;
        }
        
        taskItem.Status = newStatus;
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<TaskItemDto>(taskItem);
    }
}