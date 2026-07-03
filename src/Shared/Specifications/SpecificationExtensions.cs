namespace Shared.Abstractions.Specifications;

public static class SpecificationExtensions
{
    public static Specification<T> And<T>(this Specification<T> first, Specification<T> second)
    {
        return new AndSpecification<T>(first, second);
    }

    public static Specification<T> Or<T>(this Specification<T> first, Specification<T> second)
    {
        return new OrSpecification<T>(first, second);
    }
}
