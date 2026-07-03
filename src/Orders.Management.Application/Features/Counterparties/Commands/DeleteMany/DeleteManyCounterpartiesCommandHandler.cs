using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.Abstractions;
using Orders.Management.Domain.Entities.Counterparties;
using Shared;
using Shared.Abstractions;

namespace Orders.Management.Application.Features.Counterparties.Commands.DeleteMany;

internal sealed class DeleteManyCounterpartiesCommandHandler : ICommandHandler<DeleteManyCounterpartiesCommand, Result>
{
    private readonly IStorage<Counterparty> _storage;

    public DeleteManyCounterpartiesCommandHandler(IStorage<Counterparty> storage)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }

    public async Task<Result> Handle(DeleteManyCounterpartiesCommand command, CancellationToken cancellationToken)
    {
        await _storage.DeleteAsync(
            new FindManySpecification<Counterparty> { Ids = command.CounterpartyIds },
            cancellationToken);

        return Result.Success();
    }
}