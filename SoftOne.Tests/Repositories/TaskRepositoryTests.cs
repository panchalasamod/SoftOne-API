using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SoftOne.Data;
using SoftOne.Entities;
using SoftOne.Repositories;

namespace SoftOne.Tests.Repositories;

public class TaskRepositoryTests
{
    private static AppDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task SoftDelete_HidesEntityFromQueries()
    {
        await using var context = CreateContext(Guid.NewGuid().ToString());
        var repo = new TaskRepository(context);

        var task = await repo.AddAsync(new TaskItem
        {
            Title = "Soft delete me",
            Priority = 1,
            CreatedDate = DateTime.UtcNow,
            RowVersion = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }
        });

        await repo.SoftDeleteAsync(task);

        var found = await repo.GetByIdAsync(task.Id);
        found.Should().BeNull();
    }

    [Fact]
    public async Task GetFilteredAsync_FiltersBySearchAndStatus()
    {
        await using var context = CreateContext(Guid.NewGuid().ToString());
        var repo = new TaskRepository(context);

        await repo.AddAsync(new TaskItem
        {
            Title = "Buy milk",
            Description = "Grocery",
            Priority = 1,
            IsCompleted = false,
            CreatedDate = DateTime.UtcNow,
            RowVersion = new byte[] { 1 }
        });
        await repo.AddAsync(new TaskItem
        {
            Title = "Write docs",
            Description = "API docs",
            Priority = 3,
            IsCompleted = true,
            CreatedDate = DateTime.UtcNow,
            RowVersion = new byte[] { 2 }
        });

        var openMilk = await repo.GetFilteredAsync("milk", false, "title", false);
        openMilk.Should().ContainSingle(t => t.Title == "Buy milk");

        var completed = await repo.GetFilteredAsync(null, true, "priority", true);
        completed.Should().ContainSingle(t => t.IsCompleted);
    }
}
