using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Repositories;

public class RepositoryBase<T>(OrderContext context, ILogger<RepositoryBase<T>> logger) : IRepository<T>
    where T : BaseEntity
{
    private readonly OrderContext _context = context;
    private readonly ILogger<RepositoryBase<T>> _logger = logger;

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all {EntityType} entities", typeof(T).Name);
        return await _context.Set<T>().ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting {EntityType} entities with predicate", typeof(T).Name);
        return await _context.Set<T>().Where(predicate).ToListAsync(cancellationToken);
        
    }

    public async Task<T> GetByIdAsync(int id,CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting {EntityType} by id {EntityId}", typeof(T).Name, id);
        return await _context.Set<T>().FindAsync(id,cancellationToken);
    }

    public async Task<T> AddAsync(T entity,CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Adding new {EntityType} entity", typeof(T).Name);
        _context.Entry(entity).State = EntityState.Added;
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(T entity,CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating {EntityType} entity with id {EntityId}", typeof(T).Name, entity.Id);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity,CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting {EntityType} entity with id {EntityId}", typeof(T).Name, entity.Id);
        _context.Entry(entity).State = EntityState.Deleted;
        await _context.SaveChangesAsync(cancellationToken);
    }
}