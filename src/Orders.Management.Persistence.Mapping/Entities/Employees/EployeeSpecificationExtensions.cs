using Orders.Management.Domain.Abstractions;
using Orders.Management.Domain.Entities.Employees;
using Shared.Abstractions.Specifications;
using System.Linq.Expressions;

namespace Orders.Management.Persistence.Mapping.Entities.Employees;

internal static class EployeeSpecificationExtensions
{
    public static Expression<Func<EmployeeData, bool>> Map(this Specification<Employee> specification) => specification switch
    {
        FindByIdSpecification<Employee> s => employee => employee.Id == s.Id,
        FindManySpecification<Employee> s => employee => s.Ids.Contains(employee.Id),
        TautologySpecification<Employee> s => static employee => true,
        _ => static employee => false
    };
}
