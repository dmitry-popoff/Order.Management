using Shared;
using Shared.Abstractions;

namespace Orders.Management.Domain.Entities.Counterparties.Queries;

public sealed record GetCounterpartiesQuery(string TitleSearch, OrderingType TitleOrdering, Page Page): IFilter;

