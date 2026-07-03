using Orders.Management.Domain.DTOs;
using Orders.Management.UI.Models.Counterparties;
using Orders.Management.UI.Models.Employees;

namespace Orders.Management.UI.Models.Orders;

internal static class OrderExtensions
{
    public static OrderDetailsModel Map(this OrderDetails order ) =>
        new OrderDetailsModel
        {
            Id = order.Id,
            Sum = order.Sum,
            Counterparty = order.Counterparty.Map(),
            CreatedAt = order.CreatedAtUtc,
            Employee = order.Employee?.Map(),
            CounterpartyId = order.CounterpartyId,
            EmployeeId = order.EmployeeId,
        };

    public static OrderListModel Map(this OrderList order) =>
            new OrderListModel
            {
                Id = order.Id,
                Sum = order.Sum,
                CreatedAt = order.CreatedAtUtc,
                CounterpartyId = order.CounterpartyId,
                CounterpartyTitle = order.CounterpartyTitle ?? string.Empty,
                EmployeeId = order.EmployeeId
            };

    public static OrderListModel ToListModel(this OrderDetails details) =>
        new OrderListModel
        {
            Id = details.Id,
            Sum = details.Sum,
            CounterpartyId = details.CounterpartyId,
            CreatedAt = details.CreatedAtUtc,
            EmployeeId = details.EmployeeId,
            CounterpartyTitle = details.Counterparty?.Title ?? string.Empty
        };
}
