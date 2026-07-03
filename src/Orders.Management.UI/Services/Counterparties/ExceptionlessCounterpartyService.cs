using Microsoft.Extensions.Logging;
using Orders.Management.UI.Models.Counterparties;
using Shared;
using Shared.Abstractions;

namespace Orders.Management.UI.Services.Counterparties;

internal sealed class ExceptionlessCounterpartyService : ICounterpartyService
{
    private readonly ICounterpartyService _service;
    private readonly ILogger<ExceptionlessCounterpartyService> _logger;

    public ExceptionlessCounterpartyService(ICounterpartyService service, ILogger<ExceptionlessCounterpartyService> logger)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async ValueTask<Result> Delete(long employeeId, CancellationToken cancellationToken)
    {
        try
        {
            return await _service.Delete(employeeId, cancellationToken);
        }
        catch (Exception ex)
        {
            var message = ex.GetMessage();

            _logger.LogError(ex, message);

            return Result.Failure(new ErrorDetails(message, "Error.Unhandled"));
        }
    }

    public async ValueTask<PagedResult<CounterpartyListModel>> Get(string search, Page page, CancellationToken cancellationToken)
    {
        try
        {
            return await _service.Get(search, page, cancellationToken);
        }
        catch (Exception ex)
        {
            var message = ex.GetMessage();

            _logger.LogError(ex, message);

            return PagedResult<CounterpartyListModel>.Empty;
        }
    }

    public async ValueTask<Result<CounterpartyDetailsModel>> Find(long counterpartyId, CancellationToken cancellationToken)
    {
        try
        {
            return await _service.Find(counterpartyId,  cancellationToken);
        }
        catch (Exception ex)
        {
            var message = ex.GetMessage();

            _logger.LogError(ex, message);

            return new ErrorDetails(message, "Error.Unhandled");
        }
    }

    public async ValueTask<Result> Create(CounterpartyDetailsModel counterparty, CancellationToken cancellationToken)
    {
        try
        {
            return await _service.Create(counterparty, cancellationToken);
        }
        catch (Exception ex)
        {
            var message = ex.GetMessage();

            _logger.LogError(ex, message);

            return Result.Failure(new ErrorDetails(message, "Error.Unhandled"));
        }

    }

    public async ValueTask<Result<CounterpartyDetailsModel>> Update(long counterpartyId, long curatorId, CancellationToken cancellationToken)
    {
        try
        {
            return await _service.Update(counterpartyId, curatorId, cancellationToken);
        }
        catch (Exception ex)
        {
            var message = ex.GetMessage();

            _logger.LogError(ex, message);

            return new ErrorDetails(message, "Error.Unhandled");
        }
    }
}