using Orders.Management.Domain.Entities.Employees;

namespace Orders.Management.Persistence.Mapping.Entities.Employees;

internal class EmployeeData 
{
    public virtual long Id { get; set; }
    public virtual required string Name { get; set; }
    public virtual required string Surname { get; set; }
    public virtual required string Patronymic { get;  set; }
    public virtual PositionType Position { get; set; }
    public virtual DateTime BirthDate { get; set; }
    public virtual DateTime? DeletedAt { get; set; }
}
