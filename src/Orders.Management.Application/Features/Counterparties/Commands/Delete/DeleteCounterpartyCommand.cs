using Orders.Management.Application.Abstractions.Messaging;
using Shared;

namespace Orders.Management.Application.Features.Counterparties.Commands.Delete;

public record DeleteCounterpartyCommand(long CounterpartyId) : ICommand<Result>;
