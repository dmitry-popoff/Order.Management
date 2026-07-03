using Orders.Management.Application.Abstractions.Messaging;
using Shared;

namespace Orders.Management.Application.Features.Orders.Commands.Delete;

public sealed record DeleteOrderCommand(long OrderId) : ICommand<Result>;

