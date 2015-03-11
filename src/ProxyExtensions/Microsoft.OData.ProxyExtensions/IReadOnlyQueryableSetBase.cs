// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq.Expressions;
using Microsoft.OData.Client;

namespace Microsoft.OData.ProxyExtensions
{
    public interface IReadOnlyQueryableSetBase<TSource> : IReadOnlyQueryableSetBase
    {
        IReadOnlyQueryableSet<TSource> Expand(string expansion);

        IReadOnlyQueryableSet<TSource> Expand<TTarget>(Expression<Func<TSource, TTarget>> navigationPropertyAccessor);

        IReadOnlyQueryableSet<TResult> OfType<TResult>();

        IReadOnlyQueryableSet<TSource> OrderBy<TKey>(Expression<Func<TSource, TKey>> keySelector);

        IReadOnlyQueryableSet<TSource> OrderByDescending<TKey>(Expression<Func<TSource, TKey>> keySelector);

        IReadOnlyQueryableSet<TResult> Select<TResult>(Expression<Func<TSource, TResult>> selector);

        IReadOnlyQueryableSet<TSource> Skip(int count);

        IReadOnlyQueryableSet<TSource> Take(int count);

        IReadOnlyQueryableSet<TSource> Where(Expression<Func<TSource, bool>> predicate);
    }

    public interface IReadOnlyQueryableSetBase
    {
        DataServiceContextWrapper Context { get; }

        DataServiceQuery Query { get; }
    }
}
