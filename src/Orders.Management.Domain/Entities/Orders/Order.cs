using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Counterparties;
using Orders.Management.Domain.Entities.Employees;
using Shared;

namespace Orders.Management.Domain.Entities.Orders;

internal partial class Order: EntityBase
{
    /// <summary>
    /// Factory method for creating new object
    /// </summary>
    /// <param name="sum"></param>
    /// <param name="employee"></param>
    /// <param name="counterparty"></param>
    /// <returns></returns>
    internal static Result<Order> Create(decimal sum, Employee employee, Counterparty counterparty) => 
        new OrderBuilder()
            .WithSum(sum)
            .WithEmployee(employee)
            .WithCounterparty(counterparty)
            .Build();

    internal Order(decimal sum, Employee employee, Counterparty counterparty)
    {
        Sum = sum;
        Employee = employee;
        Counterparty = counterparty;
    }

    private Order(long orderId, decimal sum, Employee employee, Counterparty counterparty)
        : this (sum, employee, counterparty)
    {
        Id = orderId;
    }

    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
    public decimal Sum { get; private set; }
    public Employee Employee { get; private set; }
    public Counterparty Counterparty { get; private set; }

    public Result ChangeSum(decimal sum)
    {
        if (OrderContracts.OrderValidator.ValidateSum(sum, out var error)
            && error is not null)
        {
            return Result.Failure(error);
        }
        Sum = sum;

        return Result.Success();
    }

    public Result ChangeEmployee( Employee employee)
    {
        if (OrderContracts.OrderValidator.ValidateEmployee(employee, out var error)
            && error is not null) 
        {
            return Result.Failure(error);
        }
        Employee = employee;

        return Result.Success();
    }
    public OrderDetails ToDetails() =>
        new OrderDetails 
        {
            Id = Id,
            Counterparty = Counterparty.ToDetails(),
            CounterpartyId = Counterparty.Id,
            CreatedAtUtc = CreatedAtUtc,
            Employee = Employee?.ToDetails(),
            EmployeeId = Employee?.Id,
            Sum = Sum
        };
}
