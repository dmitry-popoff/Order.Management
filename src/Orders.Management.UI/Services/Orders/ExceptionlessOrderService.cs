using Microsoft.Extensions.Logging;
using Orders.Management.UI.Models.Orders;
using Shared;
using Shared.Abstractions;

namespace Orders.Management.UI.Services.Orders;

internal sealed class ExceptionlessOrderService : IOrderService
{
    private readonly IOrderService _service;
    private readonly ILogger<ExceptionlessOrderService> _logger;

    public ExceptionlessOrderService(IOrderService service, ILogger<ExceptionlessOrderService> logger)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
   
    public async ValueTask<PagedResult<OrderListModel>> Get(Page page, CancellationToken cancellationToken)
    {
        try
        {
            return await _service.Get(page, cancellationToken);
        }
        catch (Exception ex)
        {
            var message = ex.GetMessage();

            _logger.LogError(ex, message);

            return PagedResult<OrderListModel>.Empty;
        }
    }

    public async ValueTask<Result<OrderDetailsModel>> Find(long orderId, CancellationToken cancellationToken)
    {
        try
        {
            return await _service.Find(orderId, cancellationToken);
        }
        catch (Exception ex)
        {
            var message = ex.GetMessage();

            _logger.LogError(ex, message);

            return new ErrorDetails(message, "Error.Unhandled");
        }
    }

    public async ValueTask<Result<OrderDetailsModel>> Create(OrderListModel order, CancellationToken cancellationToken)
    {
        try
        {
            return await _service.Create(order, cancellationToken);
        }
        catch (Exception ex)
        {
            var message = ex.GetMessage();

            _logger.LogError(ex, message);

            return new ErrorDetails(message, "Error.Unhandled");
        }
    }
    public async ValueTask<Result<OrderDetailsModel>> Update(long orderId, long employeeId, decimal sum, CancellationToken cancellationToken)
    {
        try
        {
            return await _service.Update(orderId, employeeId, sum, cancellationToken);
        }
        catch (Exception ex)
        {
            var message = ex.GetMessage();

            _logger.LogError(ex, message);

            return new ErrorDetails(message, "Error.Unhandled");
        }
    }
    public async ValueTask<Result> Delete(long orderId, CancellationToken cancellationToken)
    {
        try
        {
            return await _service.Delete(orderId, cancellationToken);
        }
        catch (Exception ex)
        {
            var message = ex.GetMessage();

            _logger.LogError(ex, message);

            return Result.Failure(new ErrorDetails(message, "Error.Unhandled"));
        }
    }

    public async ValueTask<PagedResult<OrderListModel>> Get(Page page, string counterpartyTitle, CancellationToken cancellationToken)
    {
        try
        {
            return await _service.Get(page, counterpartyTitle, cancellationToken);
        }
        catch (Exception ex)
        {
            var message = ex.GetMessage();

            _logger.LogError(ex, message);

            return PagedResult<OrderListModel>.Empty;
        }
    }
}

