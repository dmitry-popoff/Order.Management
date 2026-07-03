using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.Abstractions;
using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Counterparties;
using Shared;

namespace Orders.Management.Application.Features.Counterparties.Queries.Find;

internal sealed class FindByIdQueryHandler(IFinder<Counterparty> finder) : IQueryHandler<FindByIdQuery, Result<CounterpartyDetails>>
{
    public async Task<Result<CounterpartyDetails>> Handle(FindByIdQuery request, CancellationToken cancellationToken)
    {
        Counterparty? entity = await finder.FindAsync(request.CounterpartyId, cancellationToken);

        if (entity is null) return ErrorDetails.NotFound;

        return entity.ToDetails();
    }
}
