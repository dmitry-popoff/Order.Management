using NHibernate;
using NHibernate.Linq;
using Shared.Abstractions;
using Shared.Abstractions.Specifications;
using System.Linq.Expressions;

namespace Orders.Management.Persistence.Mapping.Entities.Employees;

internal sealed class EmployeeDataQueryObject : IQueryObject<EmployeeData>
{
    private readonly ISessionFactory _sessionFactory;

    public EmployeeDataQueryObject(ISessionFactory sessionFactory)
    {
        _sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
    }

    public async ValueTask<PagedResult<EmployeeData>> AsPageAsync(PagedQuery<EmployeeData> pagedQuery, CancellationToken cancellationToken = default)
    {
        using ISession session = _sessionFactory.OpenSession();

        var query = session.Query<EmployeeData>()
            .Where(e => !e.DeletedAt.HasValue)
            .Where(pagedQuery.Predicate);

        IOrderedQueryable<EmployeeData> ordered = query.ApplyOrdering(pagedQuery, query.OrderBy(x => x.Surname));

        IFutureEnumerable<EmployeeData> employees = ordered.ToFuture();
        IFutureValue<int> total = query.ToFutureValue(q => q.Count());

        var employeesResult = employees
            .Skip(pagedQuery.Page.Size * (pagedQuery.Page.Number - 1))
            .Take(pagedQuery.Page.Size)
            .ToAsyncEnumerable();

        return new PagedResult<EmployeeData>
        {
            Page = pagedQuery.Page,
            Values = await employeesResult.ToArrayAsync(cancellationToken),
            Total = total.Value
        };
    }

    public ValueTask<EmployeeData[]> GetAsync(Specification<EmployeeData> specification, CancellationToken cancellationToken = default)
        => GetAsync(specification.ToExpression(), cancellationToken);
    

    public ValueTask<EmployeeData[]> GetAsync(Expression<Func<EmployeeData, bool>> predicate, CancellationToken cancellationToken = default)
    {
        using ISession session = _sessionFactory.OpenSession();

        return session.Query<EmployeeData>()
            .Where(e => !e.DeletedAt.HasValue)
            .Where(predicate)
            .ToFuture()
            .ToAsyncEnumerable()
            .ToArrayAsync(cancellationToken);
    }
}