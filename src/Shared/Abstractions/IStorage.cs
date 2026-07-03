using Shared.Abstractions.Specifications;

namespace Shared.Abstractions;

public interface IStorage<TEntity>
{
    ValueTask SaveAsync(TEntity entity, CancellationToken cancellationToken = default);
    ValueTask UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    ValueTask DeleteAsync(Specification<TEntity> specification, CancellationToken cancellationToken = default);
}
