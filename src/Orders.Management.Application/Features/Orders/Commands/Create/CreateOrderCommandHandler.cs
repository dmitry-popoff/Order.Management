using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.Abstractions;
using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Counterparties;
using Orders.Management.Domain.Entities.Employees;
using Orders.Management.Domain.Entities.Orders;
using Shared;
using Shared.Abstractions;
using static Orders.Management.Domain.Entities.Orders.OrderContracts;

namespace Orders.Management.Application.Features.Orders.Commands.Create;

internal sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Result<OrderDetails>>
{
    private readonly IFinder<Employee> _employeeFinder;
    private readonly IFinder<Counterparty> _conterpartyFinder;
    private readonly IStorage<Order> _storage;

    public CreateOrderCommandHandler(
        IFinder<Employee> employeeFinder,
        IFinder<Counterparty> conterpartyFinder,
        IStorage<Order> storage)
    {
        _employeeFinder = employeeFinder ?? throw new ArgumentNullException(nameof(employeeFinder));
        _conterpartyFinder = conterpartyFinder ?? throw new ArgumentNullException(nameof(conterpartyFinder));
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }

    public async Task<Result<OrderDetails>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var counterparty = await _conterpartyFinder.FindAsync(request.ConterpartyId, cancellationToken);

        if (counterparty is null) return Errors.CounterpartyDoesNotSet;

        var employee = await _employeeFinder.FindAsync(request.EmployeeId, cancellationToken);

        if (employee is null) return Errors.EmployeeDoesNotSet;

        var order = Order.Create(request.Sum, employee, counterparty);

        if (order.IsFailure) return order.Error;

        await _storage.SaveAsync(order.Value, cancellationToken);

        return order.Value.ToDetails();
    }
}