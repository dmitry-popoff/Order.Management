using Orders.Management.Domain.Entities;
using Shared.Abstractions.Specifications;

namespace Orders.Management.Domain.Abstractions;

public interface IFinder<TEntity>
    where TEntity : EntityBase
{
    ValueTask<TEntity?> FindAsync(long id, CancellationToken cancellationToken = default);
    ValueTask<TEntity?> FirstOrDefaultAsync(Specification<TEntity> specification, CancellationToken cancellationToken = default);
}

public interface IPagedQuery<TEntity>
    where TEntity : EntityBase
{
    ValueTask<TEntity?> FindAsync(long id, CancellationToken cancellationToken = default);
    ValueTask<TEntity?> FirstOrDefaultAsync(Specification<TEntity> specification, CancellationToken cancellationToken = default);
}
