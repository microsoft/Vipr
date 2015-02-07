using System;
using Microsoft.OData.Client;

namespace Microsoft.OData.ProxyExtensions
{
    public interface IReadOnlyQueryableSetBase<TSource> : IReadOnlyQueryableSetBase
    {
        IReadOnlyQueryableSet<TSource> Expand<TTarget>(System.Linq.Expressions.Expression<Func<TSource, TTarget>> navigationPropertyAccessor);
        IReadOnlyQueryableSet<TResult> OfType<TResult>();
        IReadOnlyQueryableSet<TSource> OrderBy<TKey>(System.Linq.Expressions.Expression<Func<TSource, TKey>> keySelector);
        IReadOnlyQueryableSet<TSource> OrderByDescending<TKey>(System.Linq.Expressions.Expression<Func<TSource, TKey>> keySelector);
        IReadOnlyQueryableSet<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<TSource, TResult>> selector);
        IReadOnlyQueryableSet<TSource> Skip(int count);
        IReadOnlyQueryableSet<TSource> Take(int count);
        IReadOnlyQueryableSet<TSource> Where(System.Linq.Expressions.Expression<Func<TSource, bool>> predicate);
    }

    public interface IReadOnlyQueryableSetBase
    {
        DataServiceContextWrapper Context { get; }
        DataServiceQuery Query { get; }
    }
}