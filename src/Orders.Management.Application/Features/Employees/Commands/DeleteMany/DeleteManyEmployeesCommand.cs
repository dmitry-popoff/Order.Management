using Orders.Management.Application.Abstractions.Messaging;
using Shared;

namespace Orders.Management.Application.Features.Employees.Commands.DeleteMany;

public record DeleteManyEmployeesCommand(long[] EmployeeIds) : ICommand<Result>;

