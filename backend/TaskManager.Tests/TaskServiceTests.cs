using FluentAssertions;
using Moq;
using TaskManager.Api.DTOs;
using TaskManager.Api.Models;
using TaskManager.Api.Repositories;
using TaskManager.Api.Services;

namespace TaskManager.Tests;

public class TaskServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateTaskWithNewStatus()
    {
        Mock<ITaskRepository> repositoryMock = new();

        repositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<TaskItem>()))
            .ReturnsAsync((TaskItem task) => task);

        TaskService service = new(repositoryMock.Object);

        CreateTaskDto dto = new()
        {
            Title = "Test task",
            Priority = TaskPriority.HIGH
        };

        TaskResponseDto result = await service.CreateAsync(dto);

        result.Title.Should().Be("Test task");
        result.Status.Should().Be(TaskStatusValue.NEW);
        result.Priority.Should().Be(TaskPriority.HIGH);
    }

    [Fact]
    public async Task GetByIdAsync_WhenTaskDoesNotExist_ShouldThrow()
    {
        Mock<ITaskRepository> repositoryMock = new();

        repositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((TaskItem?)null);

        TaskService service = new(repositoryMock.Object);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            service.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task PatchAsync_ShouldChangeStatus()
    {
        TaskItem task = new()
        {
            Id = Guid.NewGuid(),
            Title = "Old",
            Status = TaskStatusValue.NEW,
            Priority = TaskPriority.LOW,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Mock<ITaskRepository> repositoryMock = new();

        repositoryMock
            .Setup(x => x.GetByIdAsync(task.Id))
            .ReturnsAsync(task);

        TaskService service = new(repositoryMock.Object);

        PatchTaskDto dto = new()
        {
            Status = TaskStatusValue.DONE
        };

        TaskResponseDto result = await service.PatchAsync(task.Id, dto);

        result.Status.Should().Be(TaskStatusValue.DONE);
    }

    [Fact]
    public async Task PatchAsync_ShouldChangePriority()
    {
        TaskItem task = new()
        {
            Id = Guid.NewGuid(),
            Title = "Old",
            Status = TaskStatusValue.NEW,
            Priority = TaskPriority.LOW,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Mock<ITaskRepository> repositoryMock = new();

        repositoryMock
            .Setup(x => x.GetByIdAsync(task.Id))
            .ReturnsAsync(task);

        TaskService service = new(repositoryMock.Object);

        PatchTaskDto dto = new()
        {
            Priority = TaskPriority.HIGH
        };

        TaskResponseDto result = await service.PatchAsync(task.Id, dto);

        result.Priority.Should().Be(TaskPriority.HIGH);
    }

    [Fact]
    public async Task DeleteAsync_WhenTaskExists_ShouldCallRepositoryDelete()
    {
        TaskItem task = new()
        {
            Id = Guid.NewGuid(),
            Title = "Delete me"
        };

        Mock<ITaskRepository> repositoryMock = new();

        repositoryMock
            .Setup(x => x.GetByIdAsync(task.Id))
            .ReturnsAsync(task);

        TaskService service = new(repositoryMock.Object);

        await service.DeleteAsync(task.Id);

        repositoryMock.Verify(x => x.DeleteAsync(task), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateAllFields()
    {
        Guid id = Guid.NewGuid();

        TaskItem task = new()
        {
            Id = id,
            Title = "Old",
            Description = "Old description",
            Status = TaskStatusValue.NEW,
            Priority = TaskPriority.LOW
        };

        Mock<ITaskRepository> repositoryMock = new();

        repositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(task);

        TaskService service = new(repositoryMock.Object);

        UpdateTaskDto dto = new()
        {
            Title = "New title",
            Description = "New description",
            Status = TaskStatusValue.IN_PROGRESS,
            Priority = TaskPriority.HIGH
        };

        TaskResponseDto result = await service.UpdateAsync(id, dto);

        result.Title.Should().Be("New title");
        result.Description.Should().Be("New description");
        result.Status.Should().Be(TaskStatusValue.IN_PROGRESS);
        result.Priority.Should().Be(TaskPriority.HIGH);
    }
}
