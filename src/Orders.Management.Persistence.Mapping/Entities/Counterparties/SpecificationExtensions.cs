using Orders.Management.Domain.Abstractions;
using Orders.Management.Domain.Entities.Counterparties;
using Shared.Abstractions.Specifications;
using System.Linq.Expressions;

namespace Orders.Management.Persistence.Mapping.Entities.Counterparties;

internal static class SpecificationExtensions
{
    public static Expression<Func<CounterpartyData, bool>> Map(this Specification<Counterparty> specification) => specification switch
    {
        FindByIdSpecification<Counterparty> s => c => c.Id == s.Id,
        FindManySpecification<Counterparty> s => c => s.Ids.Contains(c.Id),
        TautologySpecification<Counterparty> s => static c => true,
        _ => static c => false
    };
}
