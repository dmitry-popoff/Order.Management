using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.DTOs;
using Shared;

namespace Orders.Management.Application.Features.Orders.Queries.Edit;

public sealed record EditOrderCommand(long OrderId, long EmployeeId, decimal Sum) : ICommand<Result<OrderDetails>>;
