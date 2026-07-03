using Orders.Management.Application.Abstractions.Messaging;
using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Employees;
using Shared;

namespace Orders.Management.Application.Features.Employees.Commands.Create;

public record UpdateEmployeeCommand(
    long EmployeeId,
    string Name,
    string Surname,
    string Patronymic,
    PositionType Position,
    DateTime BirthDate) : ICommand<Result<EmployeeDetails>>;
