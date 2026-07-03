using System.Linq.Expressions;

namespace Shared.Abstractions.Specifications;

public sealed class TautologySpecification<T> : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression() => static x => true;
}
public sealed class ExpressionBodySpecification<T> : Specification<T>
{
    private readonly Expression<Func<T, bool>> _criteria;
    public ExpressionBodySpecification(Expression<Func<T, bool>> criteria) => _criteria = criteria;
    public override Expression<Func<T, bool>> ToExpression() => _criteria;

    public static implicit operator ExpressionBodySpecification<T>(Expression<Func<T, bool>> criteria)
        => new ExpressionBodySpecification<T>(criteria);
}

public abstract class Specification<T>
{    
    public static TautologySpecification<T>  Default => new TautologySpecification<T>();

    public abstract Expression<Func<T, bool>> ToExpression();

    public bool IsSatisfiedBy(T entity)
    {
        Func<T, bool> predicate = ToExpression().Compile();
        return predicate(entity);
    }
}

public sealed class AndSpecification<T> : Specification<T>
{
    private readonly Specification<T> _first;
    private readonly Specification<T> _second;

    public AndSpecification(Specification<T> first, Specification<T> second) => (_first, _second) = (first, second);

    public override Expression<Func<T, bool>> ToExpression() => 
        _first.ToExpression().And(_second.ToExpression());
}

public sealed class OrSpecification<T> : Specification<T>
{
    private readonly Specification<T> _first;
    private readonly Specification<T> _second;

    public OrSpecification(Specification<T> first, Specification<T> second) => (_first, _second) = (first, second);

    public override Expression<Func<T, bool>> ToExpression() =>
        _first.ToExpression().Or(_second.ToExpression());
}