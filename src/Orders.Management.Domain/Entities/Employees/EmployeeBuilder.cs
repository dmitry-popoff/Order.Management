using Shared;
using Shared.Abstractions;
using static Orders.Management.Domain.Entities.Employees.EmployeeContracts;

namespace Orders.Management.Domain.Entities.Employees;

internal partial class Employee
{
    internal static IEditableEmployee FromStore(long employeeId) => new EmployeeBuilder(employeeId);
    internal static IEditableEmployee New() => new EmployeeBuilder();
    internal interface IEditableEmployee : IBuilder<Result<Employee>>
    {
        IEditableEmployee WithName(string name);
        IEditableEmployee WithSurname(string surname);
        IEditableEmployee WithPatronymic(string patronymic);
        IEditableEmployee WithBirthDate(DateTime birthDate);
        IEditableEmployee WithPosition(PositionType position);
    }
    private sealed class EmployeeBuilder : IEditableEmployee
    {
        private string? _name;
        private string? _surname;
        private string? _patronymic;
        private DateTime _birthDate;
        private PositionType _position;
        private long _employeeId;
        private bool _isNew = false;
        public EmployeeBuilder() { _isNew = true; }
        public EmployeeBuilder(long employeeId) => _employeeId = employeeId;

        private HashSet<ErrorDetails> errors = new();
        public IEditableEmployee WithName(string name)
        {
            if (EmployeeValidator.ValidateName(name, out var error))
            {
                _name = name;
            }
            else if (error is not null)
            {
                errors.Add(error);
            }

            return this;
        }
        public IEditableEmployee WithSurname(string surname)
        {
            if (EmployeeValidator.ValidateName(surname, out var error))
            {
                _surname = surname;
            }
            else if (error is not null)
            {
                errors.Add(error);
            }
            return this;
        }
        public IEditableEmployee WithPatronymic(string patronymic)
        {
            if (EmployeeValidator.ValidateName(patronymic, out var error))
            {
                _patronymic = patronymic;
            }
            else if (error is not null)
            {
                errors.Add(error);
            }

            return this;
        }

        public IEditableEmployee WithBirthDate(DateTime birthDate)
        {
            if (EmployeeValidator.ValidateBirthDate(birthDate, out var error))
            {
                _birthDate = birthDate;
            }
            else if (error is not null)
            {
                errors.Add(error);
            }

            return this;
        }

        public IEditableEmployee WithPosition(PositionType position)
        {
            if (EmployeeValidator.ValidatePosition(position, out var error))
            {
                _position = position;
            }
            else if (error is not null)
            {
                errors.Add(error);
            }

            return this;
        }

        public bool CanBuild => _name is not null
            && _surname is not null
            && _patronymic is not null
            && _birthDate != default
            && _position != default
            && errors.Count == 0;

        public Result<Employee> Build()
        {
            if (CanBuild is false)
                return Result<Employee>.Failure(ErrorDetails.Aggregate(errors));

            return _isNew
                ? new Employee(_surname!, _name!, _patronymic!, _birthDate, _position)
                : new Employee(_employeeId, _surname!, _name!, _patronymic!, _birthDate, _position);
        }
    }
}
