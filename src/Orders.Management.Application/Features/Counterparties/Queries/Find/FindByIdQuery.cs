using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.DTOs;
using Shared;

namespace Orders.Management.Application.Features.Counterparties.Queries.Find;

public record FindByIdQuery(long CounterpartyId) : IQuery<Result<CounterpartyDetails>>;
