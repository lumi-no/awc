using TaskManager.Api.DTOs;
using TaskManager.Api.Models;
using TaskManager.Api.Repositories;

namespace TaskManager.Api.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<TaskResponseDto> CreateAsync(CreateTaskDto dto)
    {
        TaskItem task = new()
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Status = TaskStatusValue.NEW,
            Priority = dto.Priority,
            DueDate = dto.DueDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        TaskItem created = await _repository.CreateAsync(task);
        return ToDto(created);
    }

    public async Task<List<TaskResponseDto>> GetAllAsync(TaskQueryDto query)
    {
        List<TaskItem> tasks = await _repository.GetAllAsync(query);
        return tasks.Select(ToDto).ToList();
    }

    public async Task<TaskResponseDto> GetByIdAsync(Guid id)
    {
        TaskItem? task = await _repository.GetByIdAsync(id);

        if (task == null)
        {
            throw new KeyNotFoundException("Task not found.");
        }

        return ToDto(task);
    }

    public async Task<TaskResponseDto> UpdateAsync(Guid id, UpdateTaskDto dto)
    {
        TaskItem? task = await _repository.GetByIdAsync(id);

        if (task == null)
        {
            throw new KeyNotFoundException("Task not found.");
        }

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Status = dto.Status;
        task.Priority = dto.Priority;
        task.DueDate = dto.DueDate;
        task.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(task);

        return ToDto(task);
    }

    public async Task<TaskResponseDto> PatchAsync(Guid id, PatchTaskDto dto)
    {
        TaskItem? task = await _repository.GetByIdAsync(id);

        if (task == null)
        {
            throw new KeyNotFoundException("Task not found.");
        }

        if (dto.Status.HasValue)
        {
            task.Status = dto.Status.Value;
        }

        if (dto.Priority.HasValue)
        {
            task.Priority = dto.Priority.Value;
        }

        task.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(task);

        return ToDto(task);
    }

    public async Task DeleteAsync(Guid id)
    {
        TaskItem? task = await _repository.GetByIdAsync(id);

        if (task == null)
        {
            throw new KeyNotFoundException("Task not found.");
        }

        await _repository.DeleteAsync(task);
    }

    private static TaskResponseDto ToDto(TaskItem task)
    {
        return new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }
}
