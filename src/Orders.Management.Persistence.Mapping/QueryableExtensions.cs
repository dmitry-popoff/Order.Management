using Shared.Abstractions;

namespace Orders.Management.Persistence.Mapping;

public static class QueryableExtensions
{
    public static IOrderedQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, PagedQuery<T> pagedQuery, IOrderedQueryable<T> @default)
    {
        if (pagedQuery is null) return query.Order();

        IOrderedQueryable<T> ordered = @default;

        if (pagedQuery.Order.Length > 0)
        {
            ordered = pagedQuery.Order[0].Ordering == Shared.OrderingType.Ascending
                ? query.OrderBy(pagedQuery.Order[0].OrderingSelector)
                : query.OrderByDescending(pagedQuery.Order[0].OrderingSelector);

            for (int i = 1; i < pagedQuery.Order.Length; i++)
            {
                ordered = pagedQuery.Order[i].Ordering == Shared.OrderingType.Ascending
                    ? ordered.ThenBy(pagedQuery.Order[i].OrderingSelector)
                    : ordered.ThenByDescending(pagedQuery.Order[i].OrderingSelector);
            }
        }
        ordered ??= @default;

        return ordered;
    }
}
