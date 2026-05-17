using System.Linq.Expressions;

namespace SharedKernel;

public interface ISpecification<TEntity>
{
    Expression<Func<TEntity, bool>>? Criteria { get; }
    List<Func<IQueryable<TEntity>, IQueryable<TEntity>>> IncludeChains { get; }
    Expression<Func<TEntity, object>>? OrderByExpression { get; }
    Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; }
    bool IsAsNoTracking { get; }
    bool IsAsSplitQuery { get; }
    bool IsIgnoreQueryFilters { get; }
}
