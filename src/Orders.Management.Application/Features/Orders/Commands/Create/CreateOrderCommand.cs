using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.DTOs;
using Shared;

namespace Orders.Management.Application.Features.Orders.Commands.Create;

public sealed record CreateOrderCommand(long ConterpartyId, long EmployeeId, decimal Sum) : ICommand<Result<OrderDetails>>;
