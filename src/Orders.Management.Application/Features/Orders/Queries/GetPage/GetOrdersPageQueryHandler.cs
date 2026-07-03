using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Orders.Queries;
using Shared.Abstractions;

namespace Orders.Management.Application.Features.Orders.Queries.GetPage;

public sealed class GetOrdersPageQueryHandler : IQueryHandler<GetOrdersPageQuery, PagedResult<OrderList>>
{
    private readonly IQueryExecutor<GetOrdersFilter, PagedResult<OrderList>> _queryExecutor;

    public GetOrdersPageQueryHandler(IQueryExecutor<GetOrdersFilter, PagedResult<OrderList>> queryExecutor)
    {
        _queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
    }

    public async Task<PagedResult<OrderList>> Handle(GetOrdersPageQuery request, CancellationToken cancellationToken) => 
        await _queryExecutor.ExecuteAsync(request.Query, cancellationToken);
}
