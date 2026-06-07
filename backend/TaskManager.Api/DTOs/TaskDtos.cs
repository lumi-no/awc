using TaskManager.Api.Models;

namespace TaskManager.Api.DTOs;

public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.MEDIUM;
    public DateTime? DueDate { get; set; }
}

public class UpdateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskStatusValue Status { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime? DueDate { get; set; }
}

public class PatchTaskDto
{
    public TaskStatusValue? Status { get; set; }
    public TaskPriority? Priority { get; set; }
}

public class TaskResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskStatusValue Status { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class TaskQueryDto
{
    public TaskStatusValue? Status { get; set; }
    public TaskPriority? Priority { get; set; }
    public int Limit { get; set; } = 20;
    public int Offset { get; set; } = 0;
}
