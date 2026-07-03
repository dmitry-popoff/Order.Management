using Orders.Management.Domain.Entities.Counterparties;
using Orders.Management.Domain.Entities.Employees;
using Shared;
using Shared.Abstractions;
using static Orders.Management.Domain.Entities.Orders.OrderContracts;

namespace Orders.Management.Domain.Entities.Orders;

internal partial class Order
{
    internal static IEditableOrder FromStore(long orderId) => new OrderBuilder(orderId);
    internal static IEditableOrder New() => new OrderBuilder();
    internal interface IEditableOrder : IBuilder<Result<Order>>
    {
        IEditableOrder WithEmployee(Employee employee);
        IEditableOrder WithCounterparty(Counterparty counterparty);
        IEditableOrder WithSum(decimal sum);
    }
    private sealed class OrderBuilder : IEditableOrder
    {
        private decimal _sum;
        private Employee _employee;
        private Counterparty _counterparty;

        private HashSet<ErrorDetails> errors = new();
        private readonly bool _isNew = false;
        private readonly long _orderId;

        internal OrderBuilder() { _isNew = true; }
        internal OrderBuilder(long orderId) => _orderId = orderId;
        public IEditableOrder WithEmployee(Employee employee)
        {
            if (OrderValidator.ValidateEmployee(employee, out var error))
            {
                _employee = employee;
            }
            else if (error is not null)
            {
                errors.Add(error);
            }

            return this;
        }

        public IEditableOrder WithCounterparty(Counterparty counterparty)
        {
            if (OrderValidator.ValidateCounterparty(counterparty, out var error))
            {
                _counterparty = counterparty;
            }
            else if (error is not null)
            {
                errors.Add(error);
            }

            return this;
        }
        public IEditableOrder WithSum(decimal sum)
        {
            if (OrderValidator.ValidateSum(sum, out var error))
            {
                _sum = sum;
            }
            else if (error is not null)
            {
                errors.Add(error);
            }

            return this;
        }

        public bool CanBuild => errors.Count == 0;

        public Result<Order> Build()
        {
            if (CanBuild is false)
                return Result<Order>.Failure(ErrorDetails.Aggregate(errors));

            return _isNew
                ? new Order(_sum, _employee, _counterparty)
                : new Order(_orderId, _sum, _employee, _counterparty);
        }
    }
}