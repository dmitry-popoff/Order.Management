using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.Abstractions;
using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Orders;
using Shared;

namespace Orders.Management.Application.Features.Orders.Queries.Find;

internal sealed class FindOrderQueryHandler : IQueryHandler<FindOrderQuery, Result<OrderDetails>>
{
    private readonly IFinder<Order> _finder;

    public FindOrderQueryHandler(IFinder<Order> finder)
    {
        _finder = finder ?? throw new ArgumentNullException(nameof(finder));
    }

    public async Task<Result<OrderDetails>> Handle(FindOrderQuery request, CancellationToken cancellationToken)
    {
        var entity = await _finder.FindAsync(request.OrderId, cancellationToken);

        if (entity is null) return ErrorDetails.NotFound;

        return entity.ToDetails();
    }
}