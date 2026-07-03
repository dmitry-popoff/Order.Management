using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Counterparties.Queries;
using Orders.Management.Persistence.Mapping.Entities.Counterparties;
using Shared;
using Shared.Abstractions;
using System.Linq.Expressions;

namespace Orders.Management.Persistence.Mapping.Entities.Employees.Queries;

internal class GetCounterpartyQueryExecutor : IQueryExecutor<GetCounterpartiesQuery, PagedResult<CounterpartyList>>
{
    private readonly IQueryObject<CounterpartyData> _queryObject;

    public GetCounterpartyQueryExecutor(IQueryObject<CounterpartyData> queryObject)
    {
        _queryObject = queryObject ?? throw new ArgumentNullException(nameof(queryObject));
    }

    public async ValueTask<PagedResult<CounterpartyList>> 
        ExecuteAsync(GetCounterpartiesQuery query, CancellationToken cancellationToken = default)
    {
        var page = await _queryObject.AsPageAsync(
            new PagedQuery<CounterpartyData> 
            {
                Page = query.Page,
                Predicate = BuildSearchPredicate(query),
                Order = BuildOrdering(query)
            },
            cancellationToken);

        return new PagedResult<CounterpartyList>
        {
            Page = page.Page,
            Total = page.Total,
            Values = page.Values.Select(x => x.ToList()).ToArray()
        };
    }

    private (OrderingType Ordering, Expression<Func<CounterpartyData, object>> OrderingSelector)[] 
        BuildOrdering(GetCounterpartiesQuery query)
    {
        Span<OrderingType> orderings = stackalloc[] 
        { 
            query.TitleOrdering
        };

        return [(orderings[0], static x => x.Title)];
    }

    private Expression<Func<CounterpartyData, bool>> BuildSearchPredicate(GetCounterpartiesQuery query)
    {
        Expression<Func<CounterpartyData, bool>> predicate = static x => true;

        if (string.IsNullOrWhiteSpace(query?.TitleSearch) is false)
        {
            predicate = x => x.Title.ToLower().StartsWith(query.TitleSearch.ToLower());
        }

        return predicate;
    }
}
