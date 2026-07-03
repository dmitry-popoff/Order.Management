using NHibernate;
using NHibernate.Linq;
using Shared.Abstractions;
using Shared.Abstractions.Specifications;
using System.Linq.Expressions;

namespace Orders.Management.Persistence.Mapping.Entities.Counterparties;

internal class CounterpartyQueryObject: IQueryObject<CounterpartyData>
{
    private readonly ISessionFactory _sessionFactory;

    public CounterpartyQueryObject(ISessionFactory sessionFactory)
    {
        _sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
    }

    public async ValueTask<PagedResult<CounterpartyData>> AsPageAsync(PagedQuery<CounterpartyData> pagedQuery, CancellationToken cancellationToken = default)
    {
        using ISession session = _sessionFactory.OpenSession();

        var query = session.Query<CounterpartyData>()
            .Where(e => !e.DeletedAt.HasValue)
            .Where(pagedQuery.Predicate);

        IOrderedQueryable<CounterpartyData> ordered = query.ApplyOrdering(pagedQuery, query.OrderBy(x => x.Title));

        IFutureEnumerable<CounterpartyData> entities = ordered.ToFuture();
        IFutureValue<int> total = query.ToFutureValue(q => q.Count());

        var result = entities
            .Skip(pagedQuery.Page.Size * (pagedQuery.Page.Number - 1))
            .Take(pagedQuery.Page.Size)
            .ToAsyncEnumerable();

        return new PagedResult<CounterpartyData>
        {
            Page = pagedQuery.Page,
            Values = await result.ToArrayAsync(cancellationToken),
            Total = total.Value
        };
    }

    public ValueTask<CounterpartyData[]> GetAsync(Specification<CounterpartyData> specification, CancellationToken cancellationToken = default)
        => GetAsync(specification.ToExpression(), cancellationToken);


    public ValueTask<CounterpartyData[]> GetAsync(Expression<Func<CounterpartyData, bool>> predicate, CancellationToken cancellationToken = default)
    {
        using ISession session = _sessionFactory.OpenSession();

        return session.Query<CounterpartyData>()
            .Where(e => !e.DeletedAt.HasValue)
            .Where(predicate)
            .ToFuture()
            .ToAsyncEnumerable()
            .ToArrayAsync(cancellationToken);
    }
}