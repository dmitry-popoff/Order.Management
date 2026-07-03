using Shared.Abstractions.Specifications;
using System.Linq.Expressions;

namespace Shared.Abstractions;
public interface IQueryBuilder<T>
{
    IQueryable<T> Build();
    IQueryBuilder<T> Clear();
    IQueryable<T> Query {  get; }   
    IQueryBuilder<T> AddFiltering(Expression<Func<T, bool>>[] predicates);

    IQueryBuilder<T> AddOrderingAsc(Expression<Func<T, object>> orderBy);
    IQueryBuilder<T> AddOrderingDesc(Expression<Func<T, object>> orderBy);
    IQueryBuilder<T> AddPagination(int page, int size);
    IQueryBuilder<T> ApplySpecification(Specification<T> specification);
    IQueryBuilder<T> With(Expression<Func<T, object>> property);
    IQueryBuilder<T> AddFiltering(Expression<Func<T, bool>> filter);
    IQueryBuilder<T> Split();
}


public interface IQueryBuilder<TQuery, TResult>
{
    IQueryable<TResult> Build(TQuery query);
}