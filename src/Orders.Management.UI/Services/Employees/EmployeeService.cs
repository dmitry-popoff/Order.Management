using MediatR;
using Orders.Management.Application.Features.Employees.Commands.Delete;
using Orders.Management.Application.Features.Employees.Queries.Find;
using Orders.Management.Application.Features.Employees.Queries.GetPage;
using Orders.Management.Domain.Entities.Employees.Queries;
using Orders.Management.UI.Models.Employees;
using Shared;
using Shared.Abstractions;
using Shared.Helpers;
using System.Buffers;

namespace Orders.Management.UI.Services.Employees;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly ISender _sender;

    public EmployeeService(ISender sender)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }

    public async ValueTask<PagedResult<EmployeeListModel>> Get(string search, Page page, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested) return PagedResult<EmployeeListModel>.Empty;

        var input = string.IsNullOrWhiteSpace(search) ? string.Empty : search;

        var span = StringHelpers.Split(input.AsMemory()).FirstOrDefault();

        var query = new GetEmployeesByNameQuery(
            span.IsEmpty ? string.Empty : span.ToString(),
            OrderingType.Ascending,
            null,
            OrderingType.Ascending,
            null,
            OrderingType.Ascending,
            page
            );
        var request = new GetEmployeesPageQuery(page, query);

        var result = await _sender.Send(request, cancellationToken);

        return new PagedResult<EmployeeListModel>
        {
            Total = result.Total,
            Page = result.Page,
            Values = result.Values.Length > 0
                ? result.Values
                    .Select(x => new EmployeeListModel { FullName = x.FullName, Id = x.Id, Position = x.Position})
                    .ToArray()
                : Array.Empty<EmployeeListModel>()
        };
    }

    private void ProcessNameInput(EmployeeDetailsModel employee)
    {
        var name = employee.Name.AsMemory();
        if (StringHelpers.ToUpper(ref name)) employee.Name = name.ToString();
        name = employee.Surname.AsMemory();
        if (StringHelpers.ToUpper(ref name)) employee.Surname = name.ToString();
        name = employee.Patronymic.AsMemory();
        if (StringHelpers.ToUpper(ref name)) employee.Patronymic = name.ToString();
    }

    public async ValueTask<Result> Create(EmployeeDetailsModel employee, CancellationToken cancellationToken)
    {
        ProcessNameInput(employee);

        var result = await _sender.Send(employee.ToCreateCommand(), cancellationToken);

        return result.IsFailure ? Result.Failure(result.Error) : Result.Success();
    }

    public async ValueTask<Result> Update(EmployeeDetailsModel employee, CancellationToken cancellationToken)
    {
        ProcessNameInput(employee);

        var result = await _sender.Send(employee.ToUpdateCommand(), cancellationToken);

        return result.IsFailure ? Result.Failure(result.Error) : Result.Success();
    }

    public async ValueTask<Result<EmployeeDetailsModel>> Find(long employeeId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new FindByIdQuery(employeeId), cancellationToken);

        if (result.IsFailure) return result.Error;

        return result.Value.Map();
    }

    public async ValueTask<Result> Delete(long employeeId, CancellationToken cancellationToken) =>
        await _sender.Send(new DeleteEmployeeCommand(employeeId), cancellationToken);
}
