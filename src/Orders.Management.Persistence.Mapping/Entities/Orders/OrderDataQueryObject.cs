using NHibernate;
using NHibernate.Linq;
using Shared.Abstractions;
using Shared.Abstractions.Specifications;
using System.Linq.Expressions;

namespace Orders.Management.Persistence.Mapping.Entities.Orders;

internal class OrderDataQueryObject : IQueryObject<OrderData>
{
    private readonly ISessionFactory _sessionFactory;

    public OrderDataQueryObject(ISessionFactory sessionFactory)
    {
        _sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
    }

    public async ValueTask<PagedResult<OrderData>> AsPageAsync(PagedQuery<OrderData> pagedQuery, CancellationToken cancellationToken = default)
    {
        using ISession session = _sessionFactory.OpenSession();

        var query = session.Query<OrderData>()
            .Where(e => !e.DeletedAt.HasValue)
            .Where(pagedQuery.Predicate)
            .Fetch(o => o.Counterparty)
            ;

        IOrderedQueryable<OrderData> ordered = query.ApplyOrdering(pagedQuery, query.OrderByDescending(x => x.CreatedAtUtc));

        IFutureEnumerable<OrderData> orders = ordered.ToFuture();
        IFutureValue<int> total = query.ToFutureValue(q => q.Count());

        var ordersResult = orders
            .Skip(pagedQuery.Page.Size * (pagedQuery.Page.Number - 1))
            .Take(pagedQuery.Page.Size)
            .ToAsyncEnumerable();

        return new PagedResult<OrderData>
        {
            Page = pagedQuery.Page,
            Values = await ordersResult.ToArrayAsync(cancellationToken),
            Total = total.Value
        };
    }

    public ValueTask<OrderData[]> GetAsync(Specification<OrderData> specification, CancellationToken cancellationToken = default)
        => GetAsync(specification.ToExpression(), cancellationToken);


    public ValueTask<OrderData[]> GetAsync(Expression<Func<OrderData, bool>> predicate, CancellationToken cancellationToken = default)
    {
        using ISession session = _sessionFactory.OpenSession();

        return session.Query<OrderData>()
            .Where(e => !e.DeletedAt.HasValue)
            .Where(predicate)
            .Fetch(o => o.Counterparty)
            .ToFuture()
            .ToAsyncEnumerable()
            .ToArrayAsync(cancellationToken);
    }
}

