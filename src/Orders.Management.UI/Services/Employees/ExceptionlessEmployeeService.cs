using Microsoft.Extensions.Logging;
using Orders.Management.UI.Models.Employees;
using Shared;
using Shared.Abstractions;

namespace Orders.Management.UI.Services.Employees;

internal sealed class ExceptionlessEmployeeService : IEmployeeService
{
    private readonly IEmployeeService _service;
    private readonly ILogger<ExceptionlessEmployeeService> _logger;

    public ExceptionlessEmployeeService(IEmployeeService service, ILogger<ExceptionlessEmployeeService> logger)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async ValueTask<Result> Create(EmployeeDetailsModel employee, CancellationToken cancellationToken)
    {
        try
        {
            return await _service.Create(employee, cancellationToken);
        }
        catch (Exception ex)
        {
            var message = ex.GetMessage();

            _logger.LogError(ex, message);

            return Result.Failure(new ErrorDetails(message, "Error.Unhandled"));
        }
    }

    public async ValueTask<Result> Update(EmployeeDetailsModel employee, CancellationToken cancellationToken)
    {
        try
        {
            return await _service.Update(employee, cancellationToken);
        }
        catch (Exception ex)
        {
            var message = ex.GetMessage();

            _logger.LogError(ex, message);

            return Result.Failure(new ErrorDetails(message, "Error.Unhandled"));
        }
    }
    public async ValueTask<Result<EmployeeDetailsModel>> Find(long employeeId, CancellationToken cancellationToken)
    {
        try
        {
            return await _service.Find(employeeId, cancellationToken);
        }
        catch (Exception ex) 
        {
            var message = ex.GetMessage();

            _logger.LogError(ex, message);

            return new ErrorDetails(message, "Error.Unhandled");
        }
    }

    public async ValueTask<PagedResult<EmployeeListModel>> Get(string search, Page page, CancellationToken cancellationToken)
    {
        try
        {
            return await _service.Get(search, page, cancellationToken);
        }
        catch (Exception ex)
        {
            var message = ex.GetMessage();

            _logger.LogError(ex, message);

            return PagedResult<EmployeeListModel>.Empty;
        }
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
}
