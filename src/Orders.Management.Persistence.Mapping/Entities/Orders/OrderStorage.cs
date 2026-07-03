using NHibernate;
using NHibernate.Linq;
using Orders.Management.Domain.Entities.Counterparties;
using Orders.Management.Domain.Entities.Orders;
using Orders.Management.Persistence.Mapping.Entities.Counterparties;
using Orders.Management.Persistence.Mapping.Entities.Employees;
using Shared.Abstractions;
using Shared.Abstractions.Specifications;

namespace Orders.Management.Persistence.Mapping.Entities.Orders;

internal class OrderStorage : IStorage<Order>
{
    private readonly ISessionFactory _sessionFactory;

    public OrderStorage(ISessionFactory sessionFactory)
    {
        _sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
    }

    public async ValueTask DeleteAsync(Specification<Order> criteria, CancellationToken cancellationToken = default)
    {
        using ISession session = _sessionFactory.OpenSession();
        using ITransaction transaction = session.BeginTransaction();

        try
        {
            await session.Query<OrderData>()
                .Where(criteria.Map())
                .UpdateBuilder()
                .Set(c => c.DeletedAt, c => DateTime.UtcNow)
                .UpdateAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async ValueTask SaveAsync(Order entity, CancellationToken cancellationToken = default)
    {
        using ISession session = _sessionFactory.OpenSession();
        using ITransaction transaction = session.BeginTransaction();
        try
        {
            OrderData order = entity.Map();

            await session.SaveAsync(order, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async ValueTask UpdateAsync(Order entity, CancellationToken cancellationToken = default)
    {
        using ISession session = _sessionFactory.OpenSession();
        using ITransaction transaction = session.BeginTransaction();
        try
        {
            OrderData employee = entity.Map();

            await session.UpdateAsync(employee, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}

