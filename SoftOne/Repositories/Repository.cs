using Microsoft.EntityFrameworkCore;
using SoftOne.Data;
using SoftOne.Entities;
using SoftOne.Exceptions;
using System.Linq.Expressions;

namespace SoftOne.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext Context;
    protected readonly DbSet<T> DbSet;

    public Repository(AppDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyList<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
    }

    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
        await SaveChangesWithConcurrencyAsync(cancellationToken);
    }

    public virtual async Task UpdateWithRowVersionAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        ApplyClientRowVersionAsOriginal(entity);
        await SaveChangesWithConcurrencyAsync(cancellationToken);
    }

    public virtual async Task SoftDeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        DbSet.Update(entity);
        await SaveChangesWithConcurrencyAsync(cancellationToken);
    }

    public virtual async Task SoftDeleteWithRowVersionAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        ApplyClientRowVersionAsOriginal(entity);
        await SaveChangesWithConcurrencyAsync(cancellationToken);
    }

    public virtual async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(e => e.Id == id, cancellationToken);
    }

    public virtual IQueryable<T> Query()
    {
        return DbSet.AsQueryable();
    }


    private void ApplyClientRowVersionAsOriginal(T entity)
    {
        if (entity.RowVersion == null || entity.RowVersion.Length == 0)
        {
            throw new BusinessException("RowVersion is required.");
        }

        var clientToken = entity.RowVersion;
        var entry = Context.Entry(entity);
        if (entry.State == EntityState.Detached)
        {
            DbSet.Attach(entity);
            entry = Context.Entry(entity);
        }

        entry.Property(e => e.RowVersion).OriginalValue = clientToken;
        entry.State = EntityState.Modified;
    }

    private async Task SaveChangesWithConcurrencyAsync(CancellationToken cancellationToken)
    {
        try
        {
            await Context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConflictException(ConcurrencyMessages.AlreadyUpdated);
        }
    }
}
