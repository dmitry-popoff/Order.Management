using NHibernate;
using NHibernate.Linq;
using Orders.Management.Domain.Abstractions;
using Orders.Management.Domain.Entities.Counterparties;
using Shared.Abstractions.Specifications;

namespace Orders.Management.Persistence.Mapping.Entities.Counterparties;

internal sealed class CounterpartyFinder : IFinder<Counterparty>
{
    private readonly ISessionFactory _sessionFactory;

    public CounterpartyFinder(ISessionFactory sessionFactory)
    {
        _sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
    }

    public async ValueTask<Counterparty?> FindAsync(long id, CancellationToken cancellationToken = default)
    {
        using ISession session = _sessionFactory.OpenSession();

        var counterparty = await session
            .Query<CounterpartyData>()
            .Where(x => !x.DeletedAt.HasValue)
            .Fetch(x => x.Curator)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return counterparty is not null ? counterparty.Map() : null;
    }

    public async ValueTask<Counterparty?> FirstOrDefaultAsync(Specification<Counterparty> specification, CancellationToken cancellationToken = default)
    {
        using ISession session = _sessionFactory.OpenSession();

        var counterparty = await session
            .Query<CounterpartyData>()
            .Where(x => !x.DeletedAt.HasValue)
            .Where(specification.Map())
            .Fetch(x => x.Curator)
            .FirstOrDefaultAsync(cancellationToken);

        return counterparty is not null ? counterparty.Map() : null;
    }
}

