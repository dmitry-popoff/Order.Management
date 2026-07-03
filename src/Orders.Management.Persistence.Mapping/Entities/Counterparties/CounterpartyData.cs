using Orders.Management.Persistence.Mapping.Entities.Employees;
using Orders.Management.Persistence.Mapping.Entities.Orders;

namespace Orders.Management.Persistence.Mapping.Entities.Counterparties;

internal class CounterpartyData
{
    public virtual long Id { get; set; }
    public virtual required string Title { get; set; }
    public virtual required string TaxpayerIdentificationNumber { get; set; }
    public virtual long? CuratorId { get; set; }
    public virtual EmployeeData? Curator { get; set; }
    public virtual DateTime? DeletedAt {  get; set; }

    public virtual ISet<OrderData>? Orders { get; set; }
}



