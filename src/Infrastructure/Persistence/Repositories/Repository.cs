using Infrastructure.Persistence.Specifications;
using SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

internal abstract class Repository<T>(ApplicationDbContext dbContext)
 where T : Entity
{
    protected readonly ApplicationDbContext DbContext = dbContext;

    public virtual async Task<T?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<T>()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public void Add(T entity)
    {
        DbContext.Add(entity);
    }

    public void Update(T entity)
    {
        DbContext.Update(entity);
    }

    public virtual void Delete(T entity)
    {
        DbContext.Remove(entity);
    }

    protected IQueryable<T> ApplySpecification(Specification<T> specification)
    {
        return SpecificationEvaluator.GetQuery(DbContext.Set<T>(), specification);
    }

    public async Task<T?> GetFirstOrDefaultAsync(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<T>> ListAsync(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).AnyAsync(cancellationToken);
    }
}
