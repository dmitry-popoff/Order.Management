using NHibernate;
using NHibernate.Linq;
using Orders.Management.Domain.Abstractions;
using Orders.Management.Domain.Entities.Orders;
using Shared.Abstractions.Specifications;

namespace Orders.Management.Persistence.Mapping.Entities.Orders;

internal sealed class OrderFinder : IFinder<Order>
{
    private readonly ISessionFactory _sessionFactory;

    public OrderFinder(ISessionFactory sessionFactory)
    {
        _sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
    }

    public async ValueTask<Order?> FindAsync(long id, CancellationToken cancellationToken = default)
    {
        using ISession session = _sessionFactory.OpenSession();

        var entity = await session
            .Query<OrderData>()
            .Where(x => !x.DeletedAt.HasValue)
            .Fetch(x => x.Employee)
            .Fetch(x => x.Counterparty)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity is not null ? entity.Map() : null;
    }

    public async ValueTask<Order?> FirstOrDefaultAsync(Specification<Order> specification, CancellationToken cancellationToken = default)
    {
        using ISession session = _sessionFactory.OpenSession();

        var employee = await session
            .Query<OrderData>()
            .Where(x => !x.DeletedAt.HasValue)
            .Where(specification.Map())
            .Fetch(x => x.Employee)
            .Fetch(x => x.Counterparty)
            .FirstOrDefaultAsync(cancellationToken);

        return employee is not null ? employee.Map() : null;
    }
}

