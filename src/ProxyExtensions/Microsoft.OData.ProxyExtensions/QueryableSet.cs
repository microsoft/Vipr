// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq.Expressions;
using Microsoft.OData.Client;

namespace Microsoft.OData.ProxyExtensions
{
    public class QueryableSet<TSource> : ReadOnlyQueryableSetBase<TSource>
    {
        protected string Path;
        protected object Entity;

        public void SetContainer(Func<EntityBase> entity, string property)
        {
            // Unneeded
        }

        protected Uri GetUrl()
        {
            return new Uri(Context.BaseUri.ToString().TrimEnd('/') + "/" + Path);
        }

        protected string GetPath<TInstance>(Expression<Func<TInstance, bool>> whereExpression) where TInstance : TSource
        {
            var query =
                (DataServiceQuery) System.Linq.Queryable.Where(Context.CreateQuery<TInstance>(Path), whereExpression);

            var path = query.RequestUri.ToString().Substring(Context.BaseUri.ToString().TrimEnd('/').Length + 1);

            return path;
        }

        public QueryableSet(DataServiceQuery inner, DataServiceContextWrapper context, EntityBase entity, string path)
            : base(inner, context)
        {
            Initialize(inner, context, entity, path);
        }

        public void Initialize(DataServiceQuery inner, DataServiceContextWrapper context, EntityBase entity, string path)
        {
            Initialize(inner, context);
            Path = path;
            Entity = entity;
        }
    }
}
