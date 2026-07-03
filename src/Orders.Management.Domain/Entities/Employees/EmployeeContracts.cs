using Shared;

namespace Orders.Management.Domain.Entities.Employees;

using static EmployeeContracts.Specifications;
public partial class EmployeeContracts
{
    public static class Errors
    {
        private const string ValidationErrorCode = "Error.EmployeeValidation";
        public static ErrorDetails SomePropertiesDoesNotSet =>
            new("Not all properties were set!", ValidationErrorCode);
        public static ErrorDetails NameIsNotValid =>
            new("Name has invalid format!", ValidationErrorCode);
        public static ErrorDetails BirthDateIsNotValid =>
            new("Employee's BirthDate has invalid value!", ValidationErrorCode);
        public static ErrorDetails PositionIsNotValid =>
            new("Employee's position must be set!", ValidationErrorCode);

        public static ErrorDetails EmployeeIsDeleted =>
            new("Employee entity is deleted!", ValidationErrorCode);
    }
    public static class Specifications
    {
        public static Func<string, bool> NameSpecification =>
            static name =>
                string.IsNullOrWhiteSpace(name) is false
                && name.All(char.IsLetter);

        public static Func<DateTime, bool> BirthDateSpecification =>
            static birth =>
                birth.Year > DateTime.UtcNow.Year - 100
                && birth.Year < DateTime.UtcNow.Year - 18;
    }
    public static class EmployeeValidator
    {
        public static bool ValidateName(string name, out ErrorDetails? error)
        {
            error = null;
            if (NameSpecification.Invoke(name)) return true;

            error = Errors.NameIsNotValid;
            return false;
        }

        public static bool ValidateBirthDate(DateTime birthDate, out ErrorDetails? error)
        {
            error = null;
            if (BirthDateSpecification.Invoke(birthDate)) return true;

            error = Errors.BirthDateIsNotValid;
            return false;
        }

        public static bool ValidatePosition(PositionType position, out ErrorDetails? error)
        {
            error = null;
            if (position != default) return true;

            error = Errors.PositionIsNotValid;
            return false;
        }
    }
}

