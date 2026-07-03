using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.DTOs;
using Shared;

namespace Orders.Management.Application.Features.Orders.Queries.Find;

public sealed record FindOrderQuery(long OrderId) : IQuery<Result<OrderDetails>>;
