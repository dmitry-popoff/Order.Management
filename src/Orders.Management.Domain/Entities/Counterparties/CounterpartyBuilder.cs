using Orders.Management.Domain.Entities.Employees;
using Shared;
using Shared.Abstractions;
using static Orders.Management.Domain.Entities.Counterparties.CounterpartyContracts;

namespace Orders.Management.Domain.Entities.Counterparties;

internal partial class Counterparty
{
    internal static IEditableCounterparty FromStore(long counterpartyId) => new CounterpartyBuilder(counterpartyId);
    internal static IEditableCounterparty New() => new CounterpartyBuilder();
    internal interface IEditableCounterparty : IBuilder<Result<Counterparty>>
    {
        IEditableCounterparty WithTitle(string title);
        IEditableCounterparty WithTaxpayerIdentificationNumber(string number);
        IEditableCounterparty WithCurator(Employee curator);
    }
   
    private sealed class CounterpartyBuilder :  IEditableCounterparty
    {
        private string? _title;
        private string? _taxpayerIdentificationNumber;
        private Employee? _curator;
        private HashSet<ErrorDetails> errors = new();
        private long _counterpartyId;
        private bool _isNew = false;
        public CounterpartyBuilder() { _isNew = true; }
        public CounterpartyBuilder(long counterpartyId) => _counterpartyId = counterpartyId;

        public IEditableCounterparty WithTitle(string title)
        {
            if (CounterpartyValidator.ValidateTitle(title, out var error))
            {
                _title = title;
            }
            else if (error is not null)
            {
                errors.Add(error);
            }
            return this;
        }

        public IEditableCounterparty WithTaxpayerIdentificationNumber(string number)
        {
            if (CounterpartyValidator.ValidateTaxpayerIdentificationNumber(number, out var error))
            {
                _taxpayerIdentificationNumber = number;
            }
            else if (error is not null)
            {
                errors.Add(error);
            }

            return this;
        }

        public IEditableCounterparty WithCurator(Employee curator)
        {
            if (CounterpartyValidator.ValidateCurator(curator, out var error))
            {
                _curator = curator;
            }
            else if (error is not null)
            {
                errors.Add(error);
            }

            return this;
        }

        public bool CanBuild =>
            _title is not null
            && _taxpayerIdentificationNumber is not null
            && _curator is not null
            && errors.Count == 0;

        public Result<Counterparty> Build()
        {
            if (CanBuild is false)
                return Result<Counterparty>.Failure(ErrorDetails.Aggregate(errors));

            return _isNew
                ? new Counterparty(_title!, _taxpayerIdentificationNumber!, _curator!)
                : new Counterparty(_counterpartyId, _title!, _taxpayerIdentificationNumber!, _curator!);
        }
    }
}
