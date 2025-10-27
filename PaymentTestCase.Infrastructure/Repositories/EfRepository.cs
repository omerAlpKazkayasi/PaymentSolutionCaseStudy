using Microsoft.EntityFrameworkCore;
using PaymentIntegration.Infrastructure.Persistence;
using PaymentTestCase.Domain.Abstract;
using PaymentTestCase.Domain.Common;
using System.Linq.Expressions;

namespace PaymentTestCase.Infrastructure.Repositories;

public class EfRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly PaymentDbContext _context;
    private readonly DbSet<T> _dbSet;

    public EfRepository(PaymentDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> AddAsync(T entity, CancellationToken ct = default)
    {
        await _dbSet.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public async Task<IList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default)
    {
        var query = _dbSet.AsQueryable().Where(e => !e.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);

        return await query.ToListAsync(ct);
    }

    public async Task<bool> UpdateAsync(Guid id, T entity, CancellationToken ct = default)
    {
        var existing = await _dbSet.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (existing == null) return false;

        _context.Entry(existing).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity == null) return false;

        _dbSet.Update(entity);
        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        => await _dbSet.AnyAsync(e => e.Id == id, ct);

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default)
    {
        if (predicate == null)
        {
            return false;
        }
        return await _dbSet.AnyAsync(predicate, ct);
    }
}