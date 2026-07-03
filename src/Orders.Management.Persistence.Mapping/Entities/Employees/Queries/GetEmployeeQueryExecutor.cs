using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Employees.Queries;
using Shared;
using Shared.Abstractions;
using System.Linq.Expressions;

namespace Orders.Management.Persistence.Mapping.Entities.Employees.Queries;

internal class GetEmployeeQueryExecutor : IQueryExecutor<GetEmployeesByNameQuery, PagedResult<EmployeeDetails>>
{
    private readonly IQueryObject<EmployeeData> _employeeQuery;

    public GetEmployeeQueryExecutor(IQueryObject<EmployeeData> employeeQuery)
    {
        _employeeQuery = employeeQuery ?? throw new ArgumentNullException(nameof(employeeQuery));
    }

    public async ValueTask<PagedResult<EmployeeDetails>> ExecuteAsync(GetEmployeesByNameQuery query, CancellationToken cancellationToken = default)
    {
        var page = await _employeeQuery.AsPageAsync(
            new PagedQuery<EmployeeData> 
            {
                Page = query.Page,
                Predicate = BuildSearchPredicate(query),
                Order = BuildOrdering(query)
            },
            cancellationToken);

        return new PagedResult<EmployeeDetails>
        {
            Page = page.Page,
            Total = page.Total,
            Values = page.Values.Select(x => x.ToDetails()).ToArray()
        };
    }

    private (OrderingType Ordering, Expression<Func<EmployeeData, object>> OrderingSelector)[] 
        BuildOrdering(GetEmployeesByNameQuery query)
    {
        Span<OrderingType> orderings = stackalloc[] 
        { 
            query.SurnameOrdering, query.NameOrdering, query.PatronymicOrdering 
        };
        if (string.IsNullOrWhiteSpace(query?.NameSearch) is false 
            && string.IsNullOrWhiteSpace(query?.PatronymicSearch) is true)
        {
            return [(orderings[0], static x => x.Surname), (orderings[1], static x => x.Name)];
        }
        if (string.IsNullOrWhiteSpace(query?.NameSearch) is false
            && string.IsNullOrWhiteSpace(query?.PatronymicSearch) is false)
        {
            return [(orderings[0], static x => x.Surname), (orderings[1], static x => x.Name), (orderings[2], static x => x.Patronymic)];
        }

        return [(orderings[0], static x => x.Surname)];
    }

    private Expression<Func<EmployeeData, bool>> BuildSearchPredicate(GetEmployeesByNameQuery query)
    {
        Expression<Func<EmployeeData, bool>> predicate = static x => true;

        if (string.IsNullOrWhiteSpace(query?.SurnameSearch) is false)
        {
            predicate = x => x.Surname.ToLower().StartsWith(query.SurnameSearch.ToLower());
        }
        if (string.IsNullOrWhiteSpace(query?.NameSearch) is false)
        {
            predicate.And( x => x.Name.ToLower().StartsWith(query.NameSearch.ToLower()));
        }
        if (string.IsNullOrWhiteSpace(query?.PatronymicSearch) is false)
        {
            predicate.And(x => x.Patronymic.ToLower().StartsWith(query.PatronymicSearch.ToLower()));
        }

        return predicate;
    }
}
