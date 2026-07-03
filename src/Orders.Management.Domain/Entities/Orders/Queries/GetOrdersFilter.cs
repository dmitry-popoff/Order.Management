using Shared.Abstractions;

namespace Orders.Management.Domain.Entities.Orders.Queries;

public sealed record GetOrdersFilter(
    Page Page,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    long? CounterpartyId = null,
    long? EmployeeId = null) : IFilter;

