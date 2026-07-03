using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Counterparties;
using Shared;

namespace Orders.Management.Application.Features.Counterparties.Commands.ChangeCurator;

public record ChangeCuratorCommand(long CounterpartyId, long CuratorId) : ICommand<Result<CounterpartyDetails>>;
