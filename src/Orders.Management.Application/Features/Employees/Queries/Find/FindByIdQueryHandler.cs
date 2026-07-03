using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.Abstractions;
using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Employees;
using Shared;

namespace Orders.Management.Application.Features.Employees.Queries.Find;

internal sealed class FindByIdQueryHandler(IFinder<Employee> finder) : IQueryHandler<FindByIdQuery, Result<EmployeeDetails>>
{
    public async Task<Result<EmployeeDetails>> Handle(FindByIdQuery request, CancellationToken cancellationToken)
    {
        Employee? employee = await finder.FindAsync(request.EmployeeId, cancellationToken);

        if (employee is null) return ErrorDetails.NotFound;

        return employee.ToDetails();
    }
}
