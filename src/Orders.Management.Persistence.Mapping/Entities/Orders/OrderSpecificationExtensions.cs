using Orders.Management.Domain.Abstractions;
using Orders.Management.Domain.Entities.Orders;
using Shared.Abstractions.Specifications;
using System.Linq.Expressions;

namespace Orders.Management.Persistence.Mapping.Entities.Orders;

internal static class OrderSpecificationExtensions
{
    public static Expression<Func<OrderData, bool>> Map(this Specification<Order> specification) => specification switch
    {
        FindByIdSpecification<Order> s => employee => employee.Id == s.Id,
        FindManySpecification<Order> s => employee => s.Ids.Contains(employee.Id),
        TautologySpecification<Order> s => static employee => true,
        _ => static employee => false
    };
}
