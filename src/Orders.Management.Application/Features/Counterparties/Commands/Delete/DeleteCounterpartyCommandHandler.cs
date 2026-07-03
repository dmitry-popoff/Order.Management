using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.Abstractions;
using Orders.Management.Domain.Entities.Counterparties;
using Shared;
using Shared.Abstractions;

namespace Orders.Management.Application.Features.Counterparties.Commands.Delete;

internal sealed class DeleteCounterpartyCommandHandler : ICommandHandler<DeleteCounterpartyCommand, Result>
{
    private readonly IStorage<Counterparty> _storage;

    public DeleteCounterpartyCommandHandler(IStorage<Counterparty> storage)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }

    public async Task<Result> Handle(DeleteCounterpartyCommand command, CancellationToken cancellationToken)
    {
        await _storage.DeleteAsync(
            new FindByIdSpecification<Counterparty> { Id = command.CounterpartyId },
            cancellationToken);

        return Result.Success();
    }
}
