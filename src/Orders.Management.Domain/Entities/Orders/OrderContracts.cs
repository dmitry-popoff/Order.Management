using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Counterparties;
using Orders.Management.Domain.Entities.Employees;
using Shared;

namespace Orders.Management.Domain.Entities.Orders;

public static class OrderContracts
{
    public static class Specifications
    {
        public static Func<decimal, bool> SumSpecification => static sum => sum > 0;

        internal static Func<Employee, bool> EmployeeSpecification => static employee => true;

        public static Func<EmployeeDetails, bool> EmployeeNotNullSpecification => 
            static employee => employee is not null
            && employee.Id > 0;

        internal static Func<Counterparty, bool> CounterpartySpecification => static counterparty => counterparty is not null;
        public static Func<CounterpartyDetails, bool> CounterpartyNotNullSpecification => 
            static counterparty => counterparty is not null
            && counterparty.Id > 0;
    }
    public static class Errors
    {
        private const string ValidationErrorCode = "Error.OrderValidation";
        public static ErrorDetails EmployeeDoesNotSet =>
            new("Employee does not set!", ValidationErrorCode);
        public static ErrorDetails CounterpartyDoesNotSet =>
            new("Counterparty does not set!", ValidationErrorCode);
        public static ErrorDetails SumIsNotValid =>
            new("Sum of order must be bigger then 0!", ValidationErrorCode);
    }
    public static class OrderValidator
    {
        public static bool ValidateSum(decimal sum, out ErrorDetails? error)
        {
            error = null;
            if (Specifications.SumSpecification.Invoke(sum)) return true;

            error = Errors.SumIsNotValid;
            return false;
        }

        public static bool ValidateEmployee(EmployeeDetails employee, out ErrorDetails? error)
        {
            error = null;
            if (Specifications.EmployeeNotNullSpecification.Invoke(employee)) return true;

            error = Errors.EmployeeDoesNotSet;
            return false;
        }

        internal static bool ValidateEmployee(Employee employee, out ErrorDetails? error)
        {
            error = null;
            if (Specifications.EmployeeSpecification.Invoke(employee)) return true;

            error = Errors.EmployeeDoesNotSet;
            return false;
        }

        internal static bool ValidateCounterparty(Counterparty counterparty, out ErrorDetails? error)
        {
            error = null;
            if (Specifications.CounterpartySpecification.Invoke(counterparty)) return true;

            error = Errors.CounterpartyDoesNotSet;
            return false;
        }
    }
}
