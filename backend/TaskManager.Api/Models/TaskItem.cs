їnamespace TaskManager.Api.Models;

public enum TaskStatusValue
{
    NEW,
    IN_PROGRESS,
    DONE
}

public enum TaskPriority
{
    LOW,
    MEDIUM,
    HIGH
}

public class TaskItem
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public TaskStatusValue Status { get; set; } = TaskStatusValue.NEW;

    public TaskPriority Priority { get; set; } = TaskPriority.MEDIUM;

    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
