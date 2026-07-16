using AutoMapper;
using SoftOne.Entities;
using SoftOne.Exceptions;
using SoftOne.Models.Dtos;
using SoftOne.Models.Requests;
using SoftOne.Repositories;

namespace SoftOne.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<TaskService> _logger;

    public TaskService(ITaskRepository taskRepository, IMapper mapper, ILogger<TaskService> logger)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IReadOnlyList<TaskDto>> GetAllAsync(
        string? search = null,
        bool? isCompleted = null,
        string? sortBy = null,
        bool sortDescending = false,
        CancellationToken cancellationToken = default)
    {
        var tasks = await _taskRepository.GetFilteredAsync(
            search,
            isCompleted,
            sortBy,
            sortDescending,
            cancellationToken);

        return _mapper.Map<IReadOnlyList<TaskDto>>(tasks);
    }

    public async Task<TaskDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Task with id {id} was not found.");

        return _mapper.Map<TaskDto>(task);
    }

    public async Task<TaskDto> CreateAsync(TaskAddRequest request, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<TaskItem>(request);
        var created = await _taskRepository.AddAsync(entity, cancellationToken);
        _logger.LogInformation("Created task {TaskId}", created.Id);
        return _mapper.Map<TaskDto>(created);
    }

    public async Task<TaskDto> UpdateAsync(TaskUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _taskRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Task with id {request.Id} was not found.");

        // Maps fields + parses RowVersion (base64 → byte[]) via global AutoMapper converter
        _mapper.Map(request, entity);

        await _taskRepository.UpdateWithRowVersionAsync(entity, cancellationToken);

        var refreshed = await _taskRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Task with id {request.Id} was not found.");

        _logger.LogInformation("Updated task {TaskId}", request.Id);
        return _mapper.Map<TaskDto>(refreshed);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _taskRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Task with id {id} was not found.");

        await _taskRepository.SoftDeleteAsync(entity, cancellationToken);
        _logger.LogInformation("Soft-deleted task {TaskId}", id);
    }

    public async Task<TaskDto> MarkCompletedAsync(
        int id,
        bool isCompleted,
        CancellationToken cancellationToken = default)
    {
        var entity = await _taskRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Task with id {id} was not found.");


        entity.IsCompleted = isCompleted;
        entity.IsReOpened = !isCompleted;
        await _taskRepository.UpdateAsync(entity, cancellationToken);

        var refreshed = await _taskRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Task with id {id} was not found.");

        return _mapper.Map<TaskDto>(refreshed);
    }
}
