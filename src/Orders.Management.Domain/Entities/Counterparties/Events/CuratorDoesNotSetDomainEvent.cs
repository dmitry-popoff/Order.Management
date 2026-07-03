using Orders.Management.Domain.Abstractions;

namespace Orders.Management.Domain.Entities.Counterparties.Events;

public sealed record CuratorDoesNotSetDomainEvent(long CounterpartyId) : IDomainEvent
{
}
