using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.Abstractions;
using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Employees;
using Orders.Management.Domain.Entities.Orders;
using Shared;
using Shared.Abstractions;

namespace Orders.Management.Application.Features.Orders.Queries.Edit;

internal sealed class EditOrderCommandHandler : ICommandHandler<EditOrderCommand, Result<OrderDetails>>
{
    private readonly IFinder<Order> _finder;
    private readonly IStorage<Order> _storage;
    private readonly IFinder<Employee> _employeefinder;
    public EditOrderCommandHandler(IFinder<Order> finder, IStorage<Order> storage, IFinder<Employee> employeefinder)
    {
        _finder = finder ?? throw new ArgumentNullException(nameof(finder));
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _employeefinder = employeefinder ?? throw new ArgumentNullException(nameof(employeefinder));
    }

    public async Task<Result<OrderDetails>> Handle(EditOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await _finder.FindAsync(request.OrderId, cancellationToken);

        if (entity is null) return ErrorDetails.NotFound;

        bool hasChanges = false;

        if (entity.Employee is null 
            || (entity.Employee is not null && entity.Employee.Id != request.EmployeeId))
        {
            var empResult = await AddEmployee(entity, request.EmployeeId, cancellationToken);

            if (empResult.IsFailure) return empResult.Error;

            hasChanges = true;
        }
        if (entity.Sum != request.Sum)
        {
            entity.ChangeSum(request.Sum);

            hasChanges = true;
        }
        if (hasChanges) await _storage.UpdateAsync(entity, cancellationToken);

        return entity.ToDetails();
    }

    private async ValueTask<Result> AddEmployee(Order order, long employeeId, CancellationToken cancellationToken)
    {
        var employee = await _employeefinder.FindAsync(employeeId, cancellationToken);

        if (employee is null) return Result.Failure(ErrorDetails.NotFound);

        return order.ChangeEmployee(employee);
    }
}
