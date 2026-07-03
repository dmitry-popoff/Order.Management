using Orders.Management.Application.Features.Employees.Commands.Create;
using Orders.Management.Domain.DTOs;

namespace Orders.Management.UI.Models.Employees;

internal static class EmployeeExtensions
{
    public static EmployeeDetailsModel Map(this EmployeeDetails employee) =>
        new EmployeeDetailsModel
        {
            Id = employee.Id,
            Surname = employee.Surname,
            Name = employee.Name,
            Patronymic = employee.Patronymic,
            Position = employee.Position,
            BirthDate = employee.BirthDate
        };

    public static UpdateEmployeeCommand ToUpdateCommand(this EmployeeDetailsModel employee) =>
        new UpdateEmployeeCommand(
            EmployeeId: employee.Id, 
            Name: employee.Name,
            Surname: employee.Surname,
            Patronymic: employee.Patronymic,
            Position: employee.Position,
            BirthDate: employee.BirthDate
            );

    public static CreateEmployeeCommand ToCreateCommand(this EmployeeDetailsModel employee) =>
        new CreateEmployeeCommand(
            Name: employee.Name,
            Surname: employee.Surname,
            Patronymic: employee.Patronymic,
            Position: employee.Position,
            BirthDate: employee.BirthDate
            );
}
