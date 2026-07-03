using System.Linq.Expressions;

namespace Shared.Abstractions.Specifications;

public sealed class SpecificationBuilder<T>
{
    private Specification<T> _specification;

    public SpecificationBuilder(Specification<T> specification)
    {
        _specification = specification;
    }

    public SpecificationBuilder<T> And(Specification<T> specification) 
    {
        _specification = _specification.And(specification);
        return this;
    }

    public SpecificationBuilder<T> Or(Specification<T> specification)
    {
        _specification = _specification.Or(specification);
        return this;
    }
    public SpecificationBuilder<T> And(Expression<Func<T, bool>> criteria)
    {
        _specification = _specification.And(new ExpressionBodySpecification<T>(criteria));
        return this;
    }

    public SpecificationBuilder<T> Or(Expression<Func<T, bool>> criteria)
    {
        _specification = _specification.Or(new ExpressionBodySpecification<T>(criteria));
        return this;
    }

    public Specification<T> Build() => _specification;
}
