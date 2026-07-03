using Orders.Management.Domain.Entities;
using Shared.Abstractions.Specifications;
using System.Linq.Expressions;

namespace Orders.Management.Domain.Abstractions;

public sealed class FindByIdSpecification<T> : Specification<T>
    where T : EntityBase
{
    public long Id { get; init; }
    public override Expression<Func<T, bool>> ToExpression() => x => x.Id == Id;
}

public sealed class FindManySpecification<T> : Specification<T>
    where T : EntityBase
{
    public required long[] Ids { get; init; }
    public override Expression<Func<T, bool>> ToExpression() => x => Ids.Contains(x.Id);
}
