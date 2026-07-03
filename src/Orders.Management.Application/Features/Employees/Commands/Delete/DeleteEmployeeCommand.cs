using Orders.Management.Application.Abstractions.Messaging;
using Shared;

namespace Orders.Management.Application.Features.Employees.Commands.Delete;

public record DeleteEmployeeCommand(long EmployeeId) : ICommand<Result>;
