using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.Abstractions;
using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Employees;
using Shared;
using Shared.Abstractions;

namespace Orders.Management.Application.Features.Employees.Commands.Create;

internal sealed class UpdateEmployeeCommandHandler : ICommandHandler<UpdateEmployeeCommand, Result<EmployeeDetails>>
{
    private readonly IStorage<Employee> _storage;
    private readonly IFinder<Employee> _finder;
    public UpdateEmployeeCommandHandler(IStorage<Employee> storage, IFinder<Employee> finder)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _finder = finder ?? throw new ArgumentNullException(nameof(finder));
    }

    public async Task<Result<EmployeeDetails>> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        Employee? employee = await _finder.FindAsync(command.EmployeeId, cancellationToken);

        if (employee is null) return ErrorDetails.NotFound;

        CompareUpdate(command, employee);

        await _storage.UpdateAsync(employee, cancellationToken);

        return employee.ToDetails();
    }

    private static void CompareUpdate(UpdateEmployeeCommand command, Employee employee)
    {
        if (command.BirthDate != employee.BirthDate) employee.ChangeBirthdate(command.BirthDate);
        if (string.Compare(command.Name, employee.Name, StringComparison.Ordinal) != 0) employee.ChangeName(command.Name);
        if (string.Compare(command.Surname, employee.Surname, StringComparison.Ordinal) != 0) employee.ChangeSurname(command.Surname);
        if (string.Compare(command.Patronymic, employee.Patronymic, StringComparison.Ordinal) != 0) employee.ChangePatronymic(command.Patronymic);
        if (command.Position != employee.Position) employee.ChangePosition(command.Position);
    }
}
