using System.Linq.Expressions;

namespace PaymentTestCase.Domain.Abstract;

public interface IRepository<T> where T : IEntity
{
    Task<T> AddAsync(T entity, CancellationToken ct = default);
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default);
    Task<bool> UpdateAsync(Guid id, T entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default);
}