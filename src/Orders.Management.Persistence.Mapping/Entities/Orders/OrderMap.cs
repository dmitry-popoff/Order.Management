using FluentNHibernate.Mapping;

namespace Orders.Management.Persistence.Mapping.Entities.Orders;

internal class OrderMap : ClassMap<OrderData>
{
    public OrderMap()
    {
        Id(x => x.Id).GeneratedBy.Identity();

        Map(x => x.Sum)
            .Column("sum")
            .CustomSqlType("DECIMAL(18,2)")
            .Precision(18)
            .Scale(2)
            .Not.Nullable();
        Map(x => x.CreatedAtUtc)
            .Column("created_at")
            .CustomType("DateTime")
            .Not.Nullable();
        Map(x => x.DeletedAt)
            .Column("deleted_at")
            .CustomType("DateTime")
            .Nullable();
        Map(x => x.CounterpartyId)
            .Column("counterparty_id")
            .Not.Insert().Not.Update()
            .Not.Nullable();
        Map(x => x.EmployeeId)
            .Column("employee_id")
            .Not.Insert().Not.Update()
            .Nullable();

        References(x => x.Employee, "employee_id")
            .ForeignKey(foreignKeyName: "fk_orders_employees")
            .LazyLoad()
            .Nullable()
            .Cascade.None();
        References(x => x.Counterparty, "counterparty_id")
            .ForeignKey(foreignKeyName: "fk_orders_counterparties")
            .LazyLoad()            
            .Not.Nullable()
            .Cascade.All();

        Table("orders");
    }
}
