using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Persistence.Specifications;

internal abstract class Specification<TEntity>(Expression<Func<TEntity, bool>>? criteria)
    : ISpecification<TEntity>
    where TEntity : Entity
{
    public Expression<Func<TEntity, bool>>? Criteria { get; } = criteria;
    public List<Func<IQueryable<TEntity>, IQueryable<TEntity>>> IncludeChains { get; } = [];
    public Expression<Func<TEntity, object>>? OrderByExpression { get; private set; }
    public Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; private set; }
    public bool IsAsNoTracking { get; private set; }
    public bool IsAsSplitQuery { get; private set; }
    public bool IsIgnoreQueryFilters { get; private set; }

    protected void AddInclude(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeChain) => IncludeChains.Add(includeChain);
    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression) => IncludeChains.Add(q => q.Include(includeExpression));

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression) => OrderByExpression = orderByExpression;

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression) => OrderByDescendingExpression = orderByDescendingExpression;

    protected void UseAsNoTracking() => IsAsNoTracking = true;

    protected void UseAsSplitQuery() => IsAsSplitQuery = true;

    protected void UseIgnoreQueryFilters() => IsIgnoreQueryFilters = true;
}
