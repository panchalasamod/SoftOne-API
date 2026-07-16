using Microsoft.EntityFrameworkCore;
using SoftOne.Data;
using SoftOne.Entities;

namespace SoftOne.Repositories;

public class TaskRepository : Repository<TaskItem>, ITaskRepository
{
    public TaskRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<TaskItem>> GetFilteredAsync(
        string? search,
        bool? isCompleted,
        string? sortBy,
        bool sortDescending,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(t =>
                t.Title.ToLower().Contains(term) ||
                (t.Description != null && t.Description.ToLower().Contains(term)));
        }

        if (isCompleted.HasValue)
        {
            query = query.Where(t => t.IsCompleted == isCompleted.Value);
        }

        query = (sortBy?.ToLowerInvariant()) switch
        {
            "title" => sortDescending ? query.OrderByDescending(t => t.Title) : query.OrderBy(t => t.Title),
            "priority" => sortDescending ? query.OrderByDescending(t => t.Priority) : query.OrderBy(t => t.Priority),
            "duedate" => sortDescending ? query.OrderByDescending(t => t.DueDate) : query.OrderBy(t => t.DueDate),
            "status" or "iscompleted" => sortDescending
                ? query.OrderByDescending(t => t.IsCompleted)
                : query.OrderBy(t => t.IsCompleted),
            "createddate" => sortDescending
                ? query.OrderByDescending(t => t.CreatedDate)
                : query.OrderBy(t => t.CreatedDate),
            _ => sortDescending
                ? query.OrderByDescending(t => t.Id)
                : query.OrderBy(t => t.Id)
        };

        return await query.ToListAsync(cancellationToken);
    }
}
