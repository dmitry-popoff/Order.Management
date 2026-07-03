using NHibernate;
using NHibernate.Linq;
using Orders.Management.Domain.Entities.Employees;
using Orders.Management.Persistence.Mapping.Entities.Counterparties;
using Orders.Management.Persistence.Mapping.Entities.Orders;
using Shared.Abstractions;
using Shared.Abstractions.Specifications;

namespace Orders.Management.Persistence.Mapping.Entities.Employees;

internal sealed class EmployeeStorage : IStorage<Employee>
{
    private readonly ISessionFactory _sessionFactory;

    public EmployeeStorage(ISessionFactory sessionFactory)
    {
        _sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
    }

    public async ValueTask DeleteAsync(Specification<Employee> criteria, CancellationToken cancellationToken = default)
    {
        using ISession session = _sessionFactory.OpenSession(); 
        using ITransaction transaction = session.BeginTransaction();

        try
        {
            var predicate = criteria.Map();
            await session.Query<EmployeeData>()
                .Where(predicate)
                .UpdateBuilder()
                .Set(c => c.DeletedAt, c => DateTime.UtcNow)
                .UpdateAsync(cancellationToken);

            IQueryable<long> parentSubQuery = session.Query<EmployeeData>()
                .Where(predicate)
                .Select(p => p.Id);

            await session.Query<CounterpartyData>().AsQueryable()
                .Where(c => c.CuratorId.HasValue && parentSubQuery.Contains(c.CuratorId.Value))
                .UpdateBuilder()                
                .Set(c => c.CuratorId, c => null)
                .UpdateAsync(cancellationToken);

            await session.Query<OrderData>().AsQueryable()
                .Where(c => c.EmployeeId.HasValue && parentSubQuery.Contains(c.EmployeeId.Value))
                .UpdateBuilder()
                .Set(c => c.Employee, c => null)
                .UpdateAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async ValueTask SaveAsync(Employee entity, CancellationToken cancellationToken = default)
    {
        using ISession session = _sessionFactory.OpenSession();
        using ITransaction transaction = session.BeginTransaction();
        try
        {
            EmployeeData employee = entity.Map();

            await session.SaveAsync(employee, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async ValueTask UpdateAsync(Employee entity, CancellationToken cancellationToken = default)
    {
        using ISession session = _sessionFactory.OpenSession();
        using ITransaction transaction = session.BeginTransaction();
        try
        {
            EmployeeData employee = entity.Map();

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
