using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.DTOs;
using TaskManager.Api.Services;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _service;
    private readonly IValidator<CreateTaskDto> _createValidator;
    private readonly IValidator<UpdateTaskDto> _updateValidator;
    private readonly IValidator<TaskQueryDto> _queryValidator;

    public TasksController(
        ITaskService service,
        IValidator<CreateTaskDto> createValidator,
        IValidator<UpdateTaskDto> updateValidator,
        IValidator<TaskQueryDto> queryValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _queryValidator = queryValidator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskDto dto)
    {
        await _createValidator.ValidateAndThrowAsync(dto);

        TaskResponseDto created = await _service.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] TaskQueryDto query)
    {
        await _queryValidator.ValidateAndThrowAsync(query);

        List<TaskResponseDto> tasks = await _service.GetAllAsync(query);

        return Ok(tasks);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        TaskResponseDto task = await _service.GetByIdAsync(id);
        return Ok(task);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateTaskDto dto)
    {
        await _updateValidator.ValidateAndThrowAsync(dto);

        TaskResponseDto updated = await _service.UpdateAsync(id, dto);

        return Ok(updated);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Patch(Guid id, PatchTaskDto dto)
    {
        TaskResponseDto updated = await _service.PatchAsync(id, dto);

        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
