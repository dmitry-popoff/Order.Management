using Orders.Management.Domain.DTOs;
using Shared;
using static Orders.Management.Domain.Entities.Employees.EmployeeContracts;

namespace Orders.Management.Domain.Entities.Employees;

internal partial class Employee : EntityBase
{
    public static Employee Undefined =>
        new Employee(-1, "Undefined", "Undefined", "Undefined", DateTime.MinValue, PositionType.Undefined);
        

    /// <summary>
    /// Factory method for creating new object
    /// </summary>
    /// <param name="surname"></param>
    /// <param name="name"></param>
    /// <param name="patronymic"></param>
    /// <param name="birthDate"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static Result<Employee> Create(
        string surname,
        string name,
        string patronymic,
        DateTime birthDate,
        PositionType position)
        => new EmployeeBuilder()
            .WithSurname(surname)
            .WithName(name)
            .WithPatronymic(patronymic)
            .WithBirthDate(birthDate)
            .WithPosition(position)
            .Build();

    private Employee(long id, string surname, string name, string patronymic, DateTime birthDate, PositionType position):
        this( surname,  name,  patronymic,  birthDate,  position)
    {
        Id = id;
    }

    internal Employee(string surname, string name, string patronymic, DateTime birthDate, PositionType position) 
    {
        Surname = surname;
        Name = name;
        Patronymic = patronymic;
        BirthDate = birthDate;
        Position = position;
    }

    public string Name { get; private set; } 
    public string Surname { get; private set; }
    public string Patronymic { get; private set; }

    public string FullName => String.Join(' ', Surname, Name, Patronymic ?? string.Empty); 

    public PositionType Position { get; private set; }
    public DateTime BirthDate { get; private set; }

    public Result ChangeName(string name)
    {
        if (EmployeeValidator.ValidateName(name, out ErrorDetails? error) is false 
            && error is not null)
        {
            return Result.Failure(error);
        }

        Name = name;

        return Result.Success();
    }
    public Result ChangeSurname(string surname)
    {
        if (EmployeeValidator.ValidateName(surname, out ErrorDetails? error) is false
            && error is not null)
        {
            return Result.Failure(error);
        }

        Surname = surname;

        return Result.Success();
    }
    public Result ChangePatronymic(string patronymic)
    {
        if (EmployeeValidator.ValidateName(patronymic, out ErrorDetails? error) is false
            && error is not null)
        {
            return Result.Failure(error);
        }

        Patronymic = patronymic;

        return Result.Success();
    }

    public Result ChangePosition(PositionType position)
    {
        if (EmployeeValidator.ValidatePosition(position, out ErrorDetails? error) is false
            && error is not null)
        {
            return Result.Failure(error);
        }

        if (Position == position) return Result.Success();

        Position = position;

        return Result.Success();
    }

    public Result ChangeBirthdate(DateTime birthdate)
    {
        if (EmployeeValidator.ValidateBirthDate(birthdate, out ErrorDetails? error) is false
            && error is not null)
        {
            return Result.Failure(error);
        }

        BirthDate = birthdate;

        return Result.Success();
    }

    public EmployeeDetails ToDetails() =>
        new EmployeeDetails
        {
            Id = Id,
            BirthDate = BirthDate,
            Name = Name,
            Patronymic = Patronymic,
            Position = Position,
            Surname = Surname,
        };
}

