using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Employees.Queries;
using Shared;
using Shared.Abstractions;

namespace Orders.Management.Application.Features.Employees.Queries.GetPage;

internal sealed class GetEmployeesPageQueryHandler : IQueryHandler<GetEmployeesPageQuery, PagedResult<EmployeeDetails>>
{
    private readonly IQueryExecutor<GetEmployeesByNameQuery, PagedResult<EmployeeDetails>> _queryExecutor;

    public GetEmployeesPageQueryHandler(IQueryExecutor<GetEmployeesByNameQuery, PagedResult<EmployeeDetails>> queryExecutor)
    {
        _queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
    }

    public async Task<PagedResult<EmployeeDetails>> Handle(GetEmployeesPageQuery request, CancellationToken cancellationToken)
    {
        GetEmployeesByNameQuery query = request.ByNameQuery ?? new GetEmployeesByNameQuery
        (
            SurnameSearch: string.Empty,
            SurnameOrdering: OrderingType.Ascending,
            NameSearch: string.Empty,
            NameOrdering: OrderingType.Ascending,
            PatronymicSearch: string.Empty,
            PatronymicOrdering: OrderingType.Ascending,
            Page: request.Page
        );

        return await _queryExecutor.ExecuteAsync(query, cancellationToken);
    }
}
