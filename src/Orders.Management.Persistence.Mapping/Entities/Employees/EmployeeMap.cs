using FluentNHibernate.Mapping;
using Orders.Management.Domain.Entities.Employees;

namespace Orders.Management.Persistence.Mapping.Entities.Employees;

internal class EmployeeMap : ClassMap<EmployeeData>
{
    public EmployeeMap()
    {
        Id(x => x.Id).Column("id").GeneratedBy.Identity().UnsavedValue(0);

        Map(x => x.Name)
            .Column("name")            
            .Length(100)
            .Not.Nullable();
        Map(x => x.Surname)
            .Column("surname")
            .Index("idx_surname_employees")
            .Length(100)
            .Not.Nullable();
        Map(x => x.Patronymic)
            .Column("patronymic")
            .Length(100)
            .Not.Nullable();
        Map(x => x.Position)
            .CustomType<NHibernate.Type.EnumStringType<PositionType>>()
            .Column("position")            
            .Length(100)
            .Not.Nullable();
        Map(x => x.BirthDate)
            .Column("birth_date")
            .CustomType("DateTime")
            .Not.Nullable();
        Map(x => x.DeletedAt)
            .Column("deleted_at")
            .CustomType("DateTime")
            .Nullable();


        Table("employees");
    }
}

