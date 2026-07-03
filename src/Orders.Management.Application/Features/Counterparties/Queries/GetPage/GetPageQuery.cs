using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.DTOs;
using Shared.Abstractions;

namespace Orders.Management.Application.Features.Counterparties.Queries.GetPage;

public record GetPageQuery(Page Page, string? TitleSearch = default) : IQuery<PagedResult<CounterpartyList>>;
