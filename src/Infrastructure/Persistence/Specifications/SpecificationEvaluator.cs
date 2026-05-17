using SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Specifications;

internal static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetQuery<TEntity>(
        IQueryable<TEntity> inputQueryable,
        Specification<TEntity> specification) where TEntity : Entity
    {
        IQueryable<TEntity> queryable = inputQueryable;

        if (specification.IsIgnoreQueryFilters)
        {
            queryable = queryable.IgnoreQueryFilters();
        }

        if (specification.Criteria is not null)
        {
            queryable = queryable.Where(specification.Criteria);
        }

        queryable = specification.IncludeChains.Aggregate(
            queryable,
            (current, includeChain) => includeChain(current));

        if (specification.OrderByExpression is not null)
        {
            queryable = queryable.OrderBy(specification.OrderByExpression);
        }

        if (specification.OrderByDescendingExpression is not null)
        {
            queryable = queryable.OrderByDescending(specification.OrderByDescendingExpression);
        }

        if (specification.IsAsNoTracking)
        {
            queryable = queryable.AsNoTracking();
        }

        if (specification.IsAsSplitQuery)
        {
            queryable = queryable.AsSplitQuery();
        }

        return queryable;
    }
}
