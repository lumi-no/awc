using TaskManager.Api.DTOs;

namespace TaskManager.Api.Services;

public interface ITaskService
{
    Task<TaskResponseDto> CreateAsync(CreateTaskDto dto);
    Task<List<TaskResponseDto>> GetAllAsync(TaskQueryDto query);
    Task<TaskResponseDto> GetByIdAsync(Guid id);
    Task<TaskResponseDto> UpdateAsync(Guid id, UpdateTaskDto dto);
    Task<TaskResponseDto> PatchAsync(Guid id, PatchTaskDto dto);
    Task DeleteAsync(Guid id);
