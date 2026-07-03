using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Counterparties.Queries;
using Shared;
using Shared.Abstractions;

namespace Orders.Management.Application.Features.Counterparties.Queries.GetPage;

internal sealed class GetPageQueryHandler : IQueryHandler<GetPageQuery, PagedResult<CounterpartyList>>
{
    private readonly IQueryExecutor<GetCounterpartiesQuery, PagedResult<CounterpartyList>> _queryExecutor;

    public GetPageQueryHandler(IQueryExecutor<GetCounterpartiesQuery, PagedResult<CounterpartyList>> queryExecutor)
    {
        _queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
    }

    public async Task<PagedResult<CounterpartyList>> Handle(GetPageQuery request, CancellationToken cancellationToken)
    {
        var query = new GetCounterpartiesQuery(request.TitleSearch, OrderingType.Ascending, request.Page);

        return await _queryExecutor.ExecuteAsync(query, cancellationToken);
    }
}
