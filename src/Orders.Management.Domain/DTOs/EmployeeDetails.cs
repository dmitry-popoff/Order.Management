using Orders.Management.Domain.Entities.Employees;

namespace Orders.Management.Domain.DTOs;

public sealed class EmployeeDetails
{
    public long Id { get; init; }

    public string Name { get; init; }
    public string Surname { get; init; }
    public string Patronymic { get; init; }

    public string FullName => String.Join(' ', Surname, Name, Patronymic ?? string.Empty);

    public PositionType Position { get; init; }
    public DateTime BirthDate { get; init; }
}
