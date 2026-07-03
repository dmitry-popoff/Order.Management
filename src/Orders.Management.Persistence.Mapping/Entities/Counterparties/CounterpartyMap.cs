using FluentNHibernate.Mapping;

namespace Orders.Management.Persistence.Mapping.Entities.Counterparties;

internal class CounterpartyMap : ClassMap<CounterpartyData>
{
    public CounterpartyMap()
    {
        Id(x => x.Id).Column("id").GeneratedBy.Identity().UnsavedValue(0);

        Map(x => x.Title)
            .Column("title")
            .Unique()
            .Index("idx_title_counterparties")
            .Length(100)
            .Not.Nullable();
        Map(x => x.TaxpayerIdentificationNumber)
            .Column("taxpayer_identification_number")
            .Length(50)
            .Unique()
            .Index("idx_tin_counterparties")
            .Not.Nullable();
        Map(x => x.DeletedAt)
            .Column("deleted_at")
            .CustomType("DateTime")
            .Nullable();
        Map(x => x.CuratorId)
            .Column("curator_id")
            .Not.Insert().Not.Update()
            .Nullable();

        References(x => x.Curator, "curator_id")
            .LazyLoad()            
            .Nullable()
            .ForeignKey(foreignKeyName: "fk_counterparties_employees")
            .Cascade.None();

        HasMany(x => x.Orders)
            .LazyLoad()
            .Cascade.All()
            .Inverse(); 

        Table("counterparties");
    }
}
