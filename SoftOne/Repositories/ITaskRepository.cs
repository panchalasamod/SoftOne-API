using SoftOne.Entities;

namespace SoftOne.Repositories;

public interface ITaskRepository : IRepository<TaskItem>
{
    Task<IReadOnlyList<TaskItem>> GetFilteredAsync(
        string? search,
        bool? isCompleted,
        string? sortBy,
        bool sortDescending,
        CancellationToken cancellationToken = default);
}
