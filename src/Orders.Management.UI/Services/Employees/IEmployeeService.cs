using Orders.Management.UI.Models.Employees;
using Shared;
using Shared.Abstractions;

namespace Orders.Management.UI.Services.Employees;

public interface IEmployeeService
{
    ValueTask<PagedResult<EmployeeListModel>> Get(string search, Page page, CancellationToken cancellationToken);
    ValueTask<Result<EmployeeDetailsModel>> Find(long employeeId, CancellationToken cancellationToken);
    ValueTask<Result> Create(EmployeeDetailsModel employee, CancellationToken cancellationToken);
    ValueTask<Result> Update(EmployeeDetailsModel employee, CancellationToken cancellationToken);
    ValueTask<Result> Delete(long employeeId, CancellationToken cancellationToken);
}

