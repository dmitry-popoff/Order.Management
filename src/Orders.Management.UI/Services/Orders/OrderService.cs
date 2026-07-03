using MediatR;
using Orders.Management.Application.Features.Counterparties.Queries.GetPage;
using Orders.Management.Application.Features.Orders.Commands.Create;
using Orders.Management.Application.Features.Orders.Commands.Delete;
using Orders.Management.Application.Features.Orders.Queries.Edit;
using Orders.Management.Application.Features.Orders.Queries.Find;
using Orders.Management.Application.Features.Orders.Queries.GetPage;
using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Orders;
using Orders.Management.Domain.Entities.Orders.Queries;
using Orders.Management.UI.Models.Employees;
using Orders.Management.UI.Models.Orders;
using Shared;
using Shared.Abstractions;

namespace Orders.Management.UI.Services.Orders;

internal sealed class OrderService : IOrderService
{
    private readonly ISender _sender;

    public OrderService(ISender sender)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }

    public async ValueTask<Result<OrderDetailsModel>> Create(OrderListModel order, CancellationToken cancellationToken)
    {
        if (!order.EmployeeId.HasValue) return OrderContracts.Errors.EmployeeDoesNotSet;

        var result = await _sender
            .Send(new CreateOrderCommand(order.CounterpartyId, order.EmployeeId.Value, order.Sum), cancellationToken);
        
        return result.IsSuccess ? result.Value.Map() : result.Error;
    }

    public async ValueTask<Result<OrderDetailsModel>> Update(long orderId, long employeeId, decimal sum, CancellationToken cancellationToken)
    {
        var result = await _sender
            .Send(new EditOrderCommand(orderId, employeeId, sum), cancellationToken);

        return result.IsSuccess ? result.Value.Map() : result.Error;
    }

    public async ValueTask<Result> Delete(long orderId, CancellationToken cancellationToken) =>
        await _sender.Send(new DeleteOrderCommand(orderId), cancellationToken);

    public async ValueTask<Result<OrderDetailsModel>> Find(long orderId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new FindOrderQuery(orderId), cancellationToken);

        return result.IsSuccess ? result.Value.Map() : result.Error;
    }

    public async ValueTask<PagedResult<OrderListModel>> Get(Page page, CancellationToken cancellationToken)
    {
        var result = await _sender
            .Send(new GetOrdersPageQuery(new GetOrdersFilter(page)), cancellationToken);

        return Map(result);
    }

    private static PagedResult<OrderListModel> Map(PagedResult<OrderList> pagedResult) =>
        pagedResult.IsEmpty
            ? PagedResult<OrderListModel>.Empty
            : new PagedResult<OrderListModel>
            {
                Total = pagedResult.Total,
                Page = pagedResult.Page,
                Values = pagedResult.Values.Select(x => x.Map()).ToArray()
            };

    public async ValueTask<PagedResult<OrderListModel>> Get(Page page, string counterpartyTitle, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(counterpartyTitle)) return await Get(page, cancellationToken);

        var counterparties = await _sender.Send(
                new GetPageQuery(Page: Page.GetPage(1, 1), counterpartyTitle),
                cancellationToken);

        if (counterparties.IsEmpty) return PagedResult<OrderListModel>.Empty;

        var result = await _sender.Send(
                new GetOrdersPageQuery(new GetOrdersFilter(page, null, null, counterparties.Values[0].Id)),
                cancellationToken);

        return Map(result);
    }
}

