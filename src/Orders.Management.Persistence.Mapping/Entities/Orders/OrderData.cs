using Orders.Management.Persistence.Mapping.Entities.Counterparties;
using Orders.Management.Persistence.Mapping.Entities.Employees;

namespace Orders.Management.Persistence.Mapping.Entities.Orders;

internal class OrderData
{
    public virtual long Id { get; set; }
    public virtual DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public virtual decimal Sum { get; set; }
    public virtual long? EmployeeId { get; set; }
    public virtual EmployeeData? Employee { get; set; }
    public virtual long CounterpartyId { get; set; }
    public virtual CounterpartyData Counterparty { get; set; }
    public virtual DateTime? DeletedAt { get; set; }
}



