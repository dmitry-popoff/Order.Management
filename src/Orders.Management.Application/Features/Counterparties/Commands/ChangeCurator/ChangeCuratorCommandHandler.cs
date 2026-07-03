using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.Abstractions;
using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Counterparties;
using Orders.Management.Domain.Entities.Employees;
using Shared;
using Shared.Abstractions;
using static Orders.Management.Domain.Entities.Counterparties.CounterpartyContracts;

namespace Orders.Management.Application.Features.Counterparties.Commands.ChangeCurator;

internal sealed class ChangeCuratorCommandHandler : ICommandHandler<ChangeCuratorCommand, Result<CounterpartyDetails>>
{
    private readonly IStorage<Counterparty> _storage;
    private readonly IFinder<Counterparty> _finder;
    private readonly IFinder<Employee> _employeeFinder;

    public ChangeCuratorCommandHandler(
        IStorage<Counterparty> storage,
        IFinder<Counterparty> finder,
        IFinder<Employee> employeeFinder)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _finder = finder ?? throw new ArgumentNullException(nameof(finder));
        _employeeFinder = employeeFinder ?? throw new ArgumentNullException(nameof(employeeFinder));
    }

    public async Task<Result<CounterpartyDetails>> Handle(ChangeCuratorCommand command, CancellationToken cancellationToken)
    {
        var counterparty = await _finder.FindAsync(command.CounterpartyId, cancellationToken);

        if (counterparty is null) return ErrorDetails.NotFound;

        var curator = await _employeeFinder.FindAsync(command.CuratorId, cancellationToken);

        if (curator is null) return Errors.CuratorDoesNotSet;

        var result = counterparty.ChangeCurator(curator);
        if (result.IsSuccess)
        {
            await _storage.UpdateAsync(counterparty, cancellationToken);
        }
        return result.Value.ToDetails();
    }
}

