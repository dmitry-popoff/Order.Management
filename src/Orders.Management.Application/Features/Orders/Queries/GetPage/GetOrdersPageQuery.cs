using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Orders.Queries;
using Shared.Abstractions;

namespace Orders.Management.Application.Features.Orders.Queries.GetPage;

public sealed record GetOrdersPageQuery(GetOrdersFilter Query) : IQuery<PagedResult<OrderList>>;
