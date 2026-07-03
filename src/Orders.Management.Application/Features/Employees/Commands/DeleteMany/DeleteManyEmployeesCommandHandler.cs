using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.Abstractions;
using Orders.Management.Domain.Entities.Employees;
using Shared;
using Shared.Abstractions;

namespace Orders.Management.Application.Features.Employees.Commands.DeleteMany;

internal sealed class DeleteManyEmployeesCommandHandler : ICommandHandler<DeleteManyEmployeesCommand, Result>
{
    private readonly IStorage<Employee> _storage;

    public DeleteManyEmployeesCommandHandler(IStorage<Employee> storage)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }

    public async Task<Result> Handle(DeleteManyEmployeesCommand command, CancellationToken cancellationToken)
    {
        await _storage.DeleteAsync(
            new FindManySpecification<Employee> { Ids = command.EmployeeIds },
            cancellationToken);

        return Result.Success();
    }
}

