using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.Abstractions;
using Orders.Management.Domain.Entities.Employees;
using Shared;
using Shared.Abstractions;

namespace Orders.Management.Application.Features.Employees.Commands.Delete;

internal sealed class DeleteEmployeeCommandHandler : ICommandHandler<DeleteEmployeeCommand, Result>
{
    private readonly IStorage<Employee> _storage;

    public DeleteEmployeeCommandHandler(IStorage<Employee> storage)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }

    public async Task<Result> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
    {
        await _storage.DeleteAsync(
            new FindByIdSpecification<Employee> { Id = command.EmployeeId },
            cancellationToken);

        return Result.Success();
    }
}

