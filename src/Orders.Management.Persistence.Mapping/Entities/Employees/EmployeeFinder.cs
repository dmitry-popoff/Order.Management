using NHibernate;
using NHibernate.Linq;
using Orders.Management.Domain.Abstractions;
using Orders.Management.Domain.Entities.Employees;
using Shared.Abstractions.Specifications;

namespace Orders.Management.Persistence.Mapping.Entities.Employees;

internal sealed class EmployeeFinder : IFinder<Employee>
{
    private readonly ISessionFactory _sessionFactory;

    public EmployeeFinder(ISessionFactory sessionFactory)
    {
        _sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
    }

    public async ValueTask<Employee?> FindAsync(long id, CancellationToken cancellationToken = default)
    {
        using ISession session = _sessionFactory.OpenSession();

        var employee = await session
            .Query<EmployeeData>()
            .Where(x => !x.DeletedAt.HasValue)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return employee is not null ? employee.Map() : null;
    }    

    public async ValueTask<Employee?> FirstOrDefaultAsync(Specification<Employee> specification, CancellationToken cancellationToken = default)
    {
        using ISession session = _sessionFactory.OpenSession();

        var employee = await session
            .Query<EmployeeData>()
            .Where(x => !x.DeletedAt.HasValue)
            .Where(specification.Map())            
            .FirstOrDefaultAsync(cancellationToken);

        return employee is not null ? employee.Map() : null;
    }
}

