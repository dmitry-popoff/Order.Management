
using NHibernate;
using NHibernate.Linq;
using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Orders.Queries;
using Shared.Abstractions;
using System.Linq.Expressions;

namespace Orders.Management.Persistence.Mapping.Entities.Orders.Queries;

internal class GetOrdersQueryExecutor : IQueryExecutor<GetOrdersFilter, PagedResult<OrderList>>
{
    private readonly ISessionFactory _sessionFactory;

    public GetOrdersQueryExecutor(ISessionFactory sessionFactory)
    {
        _sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
    }

    public async ValueTask<PagedResult<OrderList>> ExecuteAsync(GetOrdersFilter filter, CancellationToken cancellationToken = default)
    {
        using ISession session = _sessionFactory.OpenSession();

        var query = session.Query<OrderData>()
            .Where(e => !e.DeletedAt.HasValue)
            .Where(BuildSearchPredicate(filter))
            .Select(x => new OrderList
            {
                Id = x.Id,
                DeletedAt = x.DeletedAt,
                CounterpartyId = x.CounterpartyId,
                CounterpartyTitle = x.Counterparty.Title,
                Sum = x.Sum,
                CreatedAtUtc = x.CreatedAtUtc,
                EmployeeId = x.EmployeeId
            })
            .OrderByDescending(x => x.CreatedAtUtc);

        IFutureEnumerable<OrderList> orders = query.ToFuture();
        IFutureValue<int> total = query.ToFutureValue(q => q.Count());

        var ordersResult = orders
            .Skip(filter.Page.Size * (filter.Page.Number - 1))
            .Take(filter.Page.Size)
            .ToAsyncEnumerable();

        return new PagedResult<OrderList>
        {
            Page = filter.Page,
            Values = await ordersResult.ToArrayAsync(cancellationToken),
            Total = total.Value
        };
    }

    private Expression<Func<OrderData, bool>> BuildSearchPredicate(GetOrdersFilter query)
    {
        Expression<Func<OrderData, bool>> predicate = static x => true;

        if (query is null) return predicate;

        if (query.EndDate.HasValue)
        {
            predicate = x => query.EndDate.Value >= x.CreatedAtUtc;
        }
        if (query.StartDate.HasValue)
        {
            predicate = x => query.StartDate.Value <= x.CreatedAtUtc;
        }
        if (query.CounterpartyId.HasValue)
        {
            predicate = x => query.CounterpartyId.Value == x.CounterpartyId;
        }
        if (query.EmployeeId.HasValue)
        {
            predicate = x => x.EmployeeId.HasValue && query.EmployeeId.Value == x.EmployeeId;
        }
        return predicate;
    }
}

