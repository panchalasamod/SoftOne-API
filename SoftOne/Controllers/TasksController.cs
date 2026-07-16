using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftOne.Models.Dtos;
using SoftOne.Models.Requests;
using SoftOne.Services;

namespace SoftOne.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TaskDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<TaskDto>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] bool? isCompleted,
        [FromQuery] string? sortBy,
        [FromQuery] bool sortDescending = false,
        CancellationToken cancellationToken = default)
    {
        var tasks = await _taskService.GetAllAsync(
            search,
            isCompleted,
            sortBy,
            sortDescending,
            cancellationToken);

        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var task = await _taskService.GetByIdAsync(id, cancellationToken);
        return Ok(task);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskDto>> Create(
        [FromBody] TaskAddRequest request,
        CancellationToken cancellationToken)
    {
        var created = await _taskService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TaskDto>> Update(
        int id,
        [FromBody] TaskUpdateRequest request,
        CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest(new { message = "Route id and body id must match." });
        }

        var updated = await _taskService.UpdateAsync(request, cancellationToken);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _taskService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:int}/complete")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskDto>> MarkComplete(
        int id,
        [FromQuery] bool isCompleted = true,
        CancellationToken cancellationToken = default)
    {
        var updated = await _taskService.MarkCompletedAsync(id, isCompleted, cancellationToken);
        return Ok(updated);
    }
}
