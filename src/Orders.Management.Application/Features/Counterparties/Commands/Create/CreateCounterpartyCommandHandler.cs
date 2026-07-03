using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.Abstractions;
using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Counterparties;
using Orders.Management.Domain.Entities.Employees;
using Shared;
using Shared.Abstractions;
using static Orders.Management.Domain.Entities.Counterparties.CounterpartyContracts;

namespace Orders.Management.Application.Features.Counterparties.Commands.Create;

internal sealed class CreateCounterpartyCommandHandler : ICommandHandler<CreateCounterpartyCommand, Result<CounterpartyDetails>>
{
    private readonly IStorage<Counterparty> _storage;
    private readonly IFinder<Employee> _employeeFinder;

    public CreateCounterpartyCommandHandler(IStorage<Counterparty> storage, IFinder<Employee> employeeFinder)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _employeeFinder = employeeFinder ?? throw new ArgumentNullException(nameof(employeeFinder));
    }

    public async Task<Result<CounterpartyDetails>> Handle(CreateCounterpartyCommand command, CancellationToken cancellationToken)
    {
        Employee? curator = await _employeeFinder.FindAsync(command.CuratorId, cancellationToken);

        if (curator is null) return Errors.CuratorDoesNotSet;

        var counterparty = Counterparty.Create(command.Title, command.TaxpayerIdentificationNumber, curator);

        if (counterparty.IsSuccess)
        {
            await _storage.SaveAsync(counterparty.Value, cancellationToken);
        }
        return counterparty.Value.ToDetails();
    }
}