using Microsoft.EntityFrameworkCore;
using SoftOne.Entities;

namespace SoftOne.Data;

public class AppDbContext : DbContext
{
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor? httpContextAccessor = null)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.ToTable("Tasks");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.RowVersion).IsRowVersion();
            entity.HasQueryFilter(e => !e.IsDeleted);
            entity.HasIndex(e => e.IsCompleted);
            entity.HasIndex(e => e.Priority);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(500);
            entity.Property(e => e.RowVersion).IsRowVersion();
            entity.HasQueryFilter(e => !e.IsDeleted);
            entity.HasIndex(e => e.Username).IsUnique();
        });
    }

    public override int SaveChanges()
    {
        ApplyAuditInfo();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInfo();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditInfo()
    {
        var userId = GetCurrentUserId();
        var utcNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = utcNow;
                entry.Entity.IsDeleted = false;
                if (userId.HasValue)
                {
                    entry.Entity.CreatedUserId = userId;
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedDate = utcNow;
                if (userId.HasValue)
                {
                    entry.Entity.UpdatedUserId = userId;
                }
            }
        }
    }

    private int? GetCurrentUserId()
    {
        var claim = _httpContextAccessor?.HttpContext?.User?.FindFirst("UserId")?.Value;
        if (int.TryParse(claim, out var userId))
        {
            return userId;
        }

        return null;
    }
}
