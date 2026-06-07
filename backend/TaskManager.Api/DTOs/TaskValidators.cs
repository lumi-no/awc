using FluentValidation;

namespace TaskManager.Api.DTOs;

public class CreateTaskValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MinimumLength(3)
            .WithMessage("Title must be at least 3 characters.")
            .MaximumLength(120)
            .WithMessage("Title must be at most 120 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must be at most 500 characters.");
    }
}

public class UpdateTaskValidator : AbstractValidator<UpdateTaskDto>
{
    public UpdateTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(120);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}

public class TaskQueryValidator : AbstractValidator<TaskQueryDto>
{
    public TaskQueryValidator()
    {
        RuleFor(x => x.Limit)
            .InclusiveBetween(1, 100)
            .WithMessage("Limit must be between 1 and 100.");

        RuleFor(x => x.Offset)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Offset must be greater than or equal to 0.");
    }
}
