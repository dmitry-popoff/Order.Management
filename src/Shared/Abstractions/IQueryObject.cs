using Shared.Abstractions.Specifications;
using System.Linq.Expressions;

namespace Shared.Abstractions;

public interface IQueryObject<TEntity>
{
    ValueTask<TEntity[]> GetAsync(Specification<TEntity> specification, CancellationToken cancellationToken = default);
    ValueTask<TEntity[]> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    ValueTask<PagedResult<TEntity>> AsPageAsync(PagedQuery<TEntity> pagedQuery, CancellationToken cancellationToken = default);
}

public interface IQueryExecutor<TQuery, TResult>
    where TQuery : IFilter
{
    ValueTask<TResult> ExecuteAsync(TQuery query, CancellationToken cancellationToken = default);    
}
