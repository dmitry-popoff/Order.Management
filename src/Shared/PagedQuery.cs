using Shared.Abstractions.Specifications;
using System.Linq.Expressions;
using System.Reflection;

namespace Shared.Abstractions;

public sealed class PagedQuery<T>  
{
    public Page Page { get; init; } = Page.Default;

    public required (OrderingType Ordering, Expression<Func<T, object>> OrderingSelector)[] Order { get; init; }
    public required Expression<Func<T, bool>> Predicate { get; init; }
}

public readonly struct PagedResult<T>
{
    public static PagedResult<T> Empty = new PagedResult<T> { Page = Page.Default, Total = 0, Values = Array.Empty<T>()};
    public Page Page { get; init; }
    public int Total {  get; init; }
    public T[] Values { get; init; }
    public bool IsEmpty => Values.Length == 0;
}

public struct PagedQueryObject<T>
{
    public Page Page { get; init; } 

    public required ValueTuple<OrderingType, PropertyInfo>[] Ordering { get; init; }
    public required Specification<T> Specification { get; init; }
}

