using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.DTOs;
using Shared;

namespace Orders.Management.Application.Features.Counterparties.Commands.Create;

public record CreateCounterpartyCommand(
    string Title,
    string TaxpayerIdentificationNumber, 
    long CuratorId) 
    : ICommand<Result<CounterpartyDetails>>;
