using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.DTOs;
using Shared;

namespace Orders.Management.Application.Features.Employees.Queries.Find;

public record FindByIdQuery(long EmployeeId) : IQuery<Result<EmployeeDetails>>;
