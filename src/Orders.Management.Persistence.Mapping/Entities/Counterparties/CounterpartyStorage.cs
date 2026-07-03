using NHibernate;
using NHibernate.Linq;
using Orders.Management.Domain.Entities.Counterparties;
using Orders.Management.Persistence.Mapping.Entities.Orders;
using Shared.Abstractions;
using Shared.Abstractions.Specifications;

namespace Orders.Management.Persistence.Mapping.Entities.Counterparties;

internal sealed class CounterpartyStorage : IStorage<Counterparty>
{
    private readonly ISessionFactory _sessionFactory;

    public CounterpartyStorage(ISessionFactory sessionFactory)
    {
        _sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
    }

    public async ValueTask DeleteAsync(Specification<Counterparty> criteria, CancellationToken cancellationToken = default)
    {
        using ISession session = _sessionFactory.OpenSession();
        using ITransaction transaction = session.BeginTransaction();

        try
        {
            var predicate = criteria.Map();
            await session.Query<CounterpartyData>()
                .Where(predicate)
                .UpdateBuilder()
                .Set(c => c.DeletedAt, c => DateTime.UtcNow)
                .UpdateAsync(cancellationToken);

            var parentSubQuery = session.Query<CounterpartyData>()
                .Where(predicate)
                .Select(p => p.Id); 
            
            await session.Query<OrderData>()
                .Where(o => parentSubQuery.Contains(o.CounterpartyId))
                .UpdateBuilder()
                .Set(o => o.DeletedAt, DateTime.UtcNow)
                .UpdateAsync(cancellationToken); 

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async ValueTask SaveAsync(Counterparty entity, CancellationToken cancellationToken = default)
    {
        using ISession session = _sessionFactory.OpenSession();
        using ITransaction transaction = session.BeginTransaction();
        try
        {
            CounterpartyData counterparty = entity.Map();

            await session.SaveAsync(counterparty, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async ValueTask UpdateAsync(Counterparty entity, CancellationToken cancellationToken = default)
    {
        using ISession session = _sessionFactory.OpenSession();
        using ITransaction transaction = session.BeginTransaction();
        try
        {
            CounterpartyData counterparty = entity.Map();

            await session.UpdateAsync(counterparty, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
