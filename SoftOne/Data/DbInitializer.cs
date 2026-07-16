using SoftOne.Entities;
using SoftOne.Services;

namespace SoftOne.Data;

public static class DbInitializer
{
    public static void Seed(AppDbContext context, IPasswordHasher passwordHasher)
    {
        if (context.Users.Any())
        {
            return;
        }

        var admin = new User
        {
            Username = "admin",
            PasswordHash = passwordHasher.Hash("admin123"),
            CreatedDate = DateTime.UtcNow,
            IsDeleted = false
        };

        var demo = new User
        {
            Username = "demo",
            PasswordHash = passwordHasher.Hash("demo123"),
            CreatedDate = DateTime.UtcNow,
            IsDeleted = false
        };

        context.Users.AddRange(admin, demo);
        context.SaveChanges();

        context.Tasks.AddRange(
            new TaskItem
            {
                Title = "Welcome to SoftOne Tasks",
                Description = "Explore the task management API and Angular UI.",
                Priority = 1,
                DueDate = DateTime.UtcNow.Date.AddDays(7),
                IsCompleted = false,
                CreatedUserId = admin.Id,
                CreatedDate = DateTime.UtcNow
            },
            new TaskItem
            {
                Title = "Review assignment requirements",
                Description = "Confirm CRUD, auth, sorting and filtering work end-to-end.",
                Priority = 2,
                DueDate = DateTime.UtcNow.Date.AddDays(3),
                IsCompleted = false,
                CreatedUserId = admin.Id,
                CreatedDate = DateTime.UtcNow
            });

        context.SaveChanges();
    }
}
