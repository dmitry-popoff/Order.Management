using Shared;
using Shared.Abstractions;

namespace Orders.Management.Domain.Entities.Employees.Queries;

public record GetEmployeesByNameQuery(
    string SurnameSearch, OrderingType SurnameOrdering,
    string? NameSearch, OrderingType NameOrdering,
    string? PatronymicSearch, OrderingType PatronymicOrdering,
    Page Page) : IFilter;
