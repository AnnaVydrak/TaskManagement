using Microsoft.AspNetCore.Mvc;
using TaskManagement.Core.Dtos;
using TaskManagement.Core.Services;

namespace TaskManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController(ITaskService taskService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var tasks = await taskService.GetAllAsync(cancellationToken);
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var task = await taskService.GetByIdAsync(id, cancellationToken);
        if (task == null)
        {
            return NotFound();
        }
    
        return Ok(task);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] TaskItemCreateDto taskDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
    
        try
        {
            return Ok(await taskService.AddAsync(taskDto, cancellationToken));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStatusAsync(int id, [FromBody] TaskItemStatus newStatus, CancellationToken cancellationToken)
    {
        try
        {
            var updatedTask = await taskService.UpdateStatusAsync(id, newStatus, cancellationToken);
            if (updatedTask == null)
            {
                return NotFound();
            }
    
            return Ok(updatedTask);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}