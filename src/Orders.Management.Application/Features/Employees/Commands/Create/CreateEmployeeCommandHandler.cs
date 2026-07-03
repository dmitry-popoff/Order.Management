using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Employees;
using Shared;
using Shared.Abstractions;

namespace Orders.Management.Application.Features.Employees.Commands.Create;

internal sealed class CreateEmployeeCommandHandler : ICommandHandler<CreateEmployeeCommand, Result<EmployeeDetails>>
{
    private readonly IStorage<Employee> _storage;

    public CreateEmployeeCommandHandler(IStorage<Employee> storage)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }

    public async Task<Result<EmployeeDetails>> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var result = Employee.Create(
            surname: command.Surname,
            name: command.Name,
            patronymic: command.Patronymic,
            position: command.Position,
            birthDate: command.BirthDate
            );
        if (result.IsFailure) return result.Error;

        await _storage.SaveAsync(result.Value, cancellationToken);

        return result.Value.ToDetails();
    }
}
