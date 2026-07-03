using Orders.Management.UI.Models.Counterparties;
using Orders.Management.UI.Models.Orders;
using Shared;
using Shared.Abstractions;

namespace Orders.Management.UI.Services.Orders;

public interface IOrderService
{
    ValueTask<PagedResult<OrderListModel>> Get(Page page, CancellationToken cancellationToken);
    ValueTask<Result<OrderDetailsModel>> Find(long orderId, CancellationToken cancellationToken);
    ValueTask<Result<OrderDetailsModel>> Create(OrderListModel order, CancellationToken cancellationToken);
    ValueTask<Result> Delete(long orderId, CancellationToken cancellationToken);
    ValueTask<PagedResult<OrderListModel>> Get(Page page, string counterpartyTitle, CancellationToken cancellationToken);
    ValueTask<Result<OrderDetailsModel>> Update(long orderId, long employeeId, decimal sum, CancellationToken cancellationToken);
}
