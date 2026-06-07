using TaskManager.Api.DTOs;
using TaskManager.Api.Models;

namespace TaskManager.Api.Repositories;

public interface ITaskRepository
{
    Task<TaskItem> CreateAsync(TaskItem task);
    Task<List<TaskItem>> GetAllAsync(TaskQueryDto query);
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(TaskItem task);
}
