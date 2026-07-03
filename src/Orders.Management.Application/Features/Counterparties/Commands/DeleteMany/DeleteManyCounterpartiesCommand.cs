using Orders.Management.Application.Abstractions.Messaging;
using Shared;

namespace Orders.Management.Application.Features.Counterparties.Commands.DeleteMany;

public record DeleteManyCounterpartiesCommand(long[] CounterpartyIds) : ICommand<Result>;
