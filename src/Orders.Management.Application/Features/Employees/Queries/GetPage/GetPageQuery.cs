using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Employees.Queries;
using Shared.Abstractions;

namespace Orders.Management.Application.Features.Employees.Queries.GetPage;

public record GetEmployeesPageQuery(Page Page, GetEmployeesByNameQuery? ByNameQuery) : IQuery<PagedResult<EmployeeDetails>>;
