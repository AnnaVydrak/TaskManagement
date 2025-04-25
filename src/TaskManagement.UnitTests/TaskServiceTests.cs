using Moq;
using TaskManagement.Core.Common.Exceptions;
using TaskManagement.Core.Common.Providers;
using TaskManagement.Core.Dtos;
using TaskManagement.Core.Repositories;
using TaskManagement.Core.Services;
using TaskManagement.Messaging;
using TaskManagement.Messaging.Events;
using Xunit;
using Assert = Xunit.Assert;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _taskRepoMock = new();
    private readonly Mock<ITaskEventProducer> _eventProducerMock = new();
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock = new();
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        _service = new TaskService(_taskRepoMock.Object, _eventProducerMock.Object, _dateTimeProviderMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsTasks()
    {
        // Arrange
        var tasks = new List<TaskItemDto> { new() { Id = 1, Name = "Task", Status = TaskItemStatus.NotStarted } };
        _taskRepoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(tasks);

        // Act
        var result = await _service.GetAllAsync(CancellationToken.None);

        // Assert
        Assert.Equal(tasks, result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsTask_WhenExists()
    {
        var task = new TaskItemDto { Id = 1, Name = "Task", Status = TaskItemStatus.NotStarted };
        _taskRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(task);

        var result = await _service.GetByIdAsync(1, CancellationToken.None);

        Assert.Equal(task, result);
    }

    [Fact]
    public async Task AddAsync_CreatesAndPublishesEvent_IfCompleted()
    {
        var task = new TaskItemDto { Id = 1, Name = "Complete Task", Status = TaskItemStatus.Completed };
        _taskRepoMock.Setup(r => r.AddAsync(It.IsAny<TaskItemCreateDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(task);
        _dateTimeProviderMock.Setup(p => p.UtcNow).Returns(DateTime.UtcNow);

        var result = await _service.AddAsync(new TaskItemCreateDto(), CancellationToken.None);

        _eventProducerMock.Verify(p => p.PublishTaskCompletedAsync(It.IsAny<TaskCompletedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(task, result);
    }

    [Fact]
    public async Task UpdateStatusAsync_UpdatesStatus_AndPublishesEvent_IfCompleted()
    {
        var originalTask = new TaskItemDto { Id = 1, Name = "Task1", Status = TaskItemStatus.InProgress };
        var updatedTask = new TaskItemDto { Id = 1, Name = "Task1", Status = TaskItemStatus.Completed };

        _taskRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(originalTask);
        _taskRepoMock.Setup(r => r.UpdateStatusAsync(1, TaskItemStatus.Completed, It.IsAny<CancellationToken>())).ReturnsAsync(updatedTask);
        _dateTimeProviderMock.Setup(p => p.UtcNow).Returns(DateTime.UtcNow);

        var result = await _service.UpdateStatusAsync(1, TaskItemStatus.Completed, CancellationToken.None);

        _eventProducerMock.Verify(p => p.PublishTaskCompletedAsync(It.IsAny<TaskCompletedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(TaskItemStatus.Completed, result?.Status);
    }

    [Fact]
    public async Task UpdateStatusAsync_Throws_WhenTaskNotFound()
    {
        _taskRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((TaskItemDto?)null);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.UpdateStatusAsync(1, TaskItemStatus.Completed, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateStatusAsync_Throws_WhenInvalidTransition()
    {
        var task = new TaskItemDto { Id = 1, Name = "Invalid", Status = TaskItemStatus.Completed };
        _taskRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(task);

        await Assert.ThrowsAsync<InvalidStatusTransitionException>(() =>
            _service.UpdateStatusAsync(1, TaskItemStatus.InProgress, CancellationToken.None));
    }
}
