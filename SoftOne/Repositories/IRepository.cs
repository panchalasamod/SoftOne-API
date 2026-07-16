using SoftOne.Entities;
using System.Linq.Expressions;

namespace SoftOne.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
 
    Task UpdateWithRowVersionAsync(T entity, CancellationToken cancellationToken = default);

    Task SoftDeleteAsync(T entity, CancellationToken cancellationToken = default);
 
    Task SoftDeleteWithRowVersionAsync(T entity, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    IQueryable<T> Query();
}
