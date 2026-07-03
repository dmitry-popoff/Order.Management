using Orders.Management.UI.Models.Counterparties;
using Shared;
using Shared.Abstractions;

namespace Orders.Management.UI.Services.Counterparties;

public interface ICounterpartyService
{
    ValueTask<PagedResult<CounterpartyListModel>> Get(string search, Page page, CancellationToken cancellationToken);
    ValueTask<Result<CounterpartyDetailsModel>> Find(long counterpartyId, CancellationToken cancellationToken);
    ValueTask<Result> Create(CounterpartyDetailsModel counterparty, CancellationToken cancellationToken);
    ValueTask<Result<CounterpartyDetailsModel>> Update(long counterpartyId, long curatorId, CancellationToken cancellationToken);
    ValueTask<Result> Delete(long counterpartyId, CancellationToken cancellationToken);
}
