using SoftOne.Models.Dtos;
using SoftOne.Models.Requests;

namespace SoftOne.Services;

public interface ITaskService
{
    Task<IReadOnlyList<TaskDto>> GetAllAsync(
        string? search = null,
        bool? isCompleted = null,
        string? sortBy = null,
        bool sortDescending = false,
        CancellationToken cancellationToken = default);

    Task<TaskDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<TaskDto> CreateAsync(TaskAddRequest request, CancellationToken cancellationToken = default);

    Task<TaskDto> UpdateAsync(TaskUpdateRequest request, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<TaskDto> MarkCompletedAsync(int id, bool isCompleted, CancellationToken cancellationToken = default);
}
