using CountryValidation;
using Orders.Management.Domain.DTOs;
using Orders.Management.Domain.Entities.Employees;
using Shared;

namespace Orders.Management.Domain.Entities.Counterparties;

public partial class CounterpartyContracts
{
    public static class Errors
    {
        private const string ValidationErrorCode = "Error.CounterpartyValidation";

        public static ErrorDetails InvalidIdentifier =>
           new("Identifier has wrong value!", ValidationErrorCode);

        public static ErrorDetails TitleIsNotValid =>
            new("Name has invalid format!", ValidationErrorCode);

        public static ErrorDetails CuratorDoesNotSet =>
            new("Curator for Counterparty Does Not Set!", ValidationErrorCode);

        public static ErrorDetails TaxpayerIdentificationNumberIsNotValid =>
            new("Taxpayer Identification Number has invalid format or value!", ValidationErrorCode);
    }

    public static class Specifications
    {
        internal static Func<Employee, bool> CuratorSpecification =>
            static curator => curator is not null;

        public static Func<EmployeeDetails, bool> CuratorNotNullSpecification =>
            static curator => curator is not null
            && curator.Id > 0;

        public static Func<string, bool> TitleSpecification =>
            static name =>
                string.IsNullOrWhiteSpace(name) is false
                ;

        private static CountryValidator validator = new CountryValidator();

        public static Func<string, bool> TaxpayerIdentificationNumberSpecification =>
            static number =>
                string.IsNullOrWhiteSpace(number) is false
                && (validator.ValidateIndividualTaxCode(number, Country.RU).IsValid
                    || validator.ValidateNationalIdentityCode(number, Country.RU).IsValid);
    }

    public static class CounterpartyValidator
    {
        internal static bool ValidateCurator(Employee curator, out ErrorDetails? error)
        {
            error = null;
            if (Specifications.CuratorSpecification.Invoke(curator)) return true;

            error = Errors.CuratorDoesNotSet;
            return false;
        }
        public static bool ValidateCurator(EmployeeDetails curator, out ErrorDetails? error)
        {
            error = null;
            if (Specifications.CuratorNotNullSpecification.Invoke(curator)) return true;

            error = Errors.CuratorDoesNotSet;
            return false;
        }

        public static bool ValidateTitle(string name, out ErrorDetails? error)
        {
            error = null;
            if (Specifications.TitleSpecification.Invoke(name)) return true;

            error = Errors.TitleIsNotValid;
            return false;
        }

        public static bool ValidateTaxpayerIdentificationNumber(string number, out ErrorDetails? error)
        {
            error = null;
            if (Specifications.TitleSpecification.Invoke(number)) return true;

            error = Errors.TaxpayerIdentificationNumberIsNotValid;
            return false;
        }
    }
}