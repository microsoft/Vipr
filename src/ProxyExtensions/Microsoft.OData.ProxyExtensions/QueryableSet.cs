using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.OData.Client;

namespace Microsoft.OData.ProxyExtensions
{
    public class QueryableSet<TSource> : ReadOnlyQueryableSetBase<TSource>
    {
        protected string _path;
        protected object _entity;

        public void SetContainer(Func<EntityBase> entity, string property)
        {
            // Unneeded
        }

        protected System.Uri GetUrl()
        {
            return new Uri(Context.BaseUri.ToString().TrimEnd('/') + "/" + _path);
        }

        protected string GetPath<TInstance>(Expression<Func<TInstance, bool>> whereExpression) where TInstance : TSource
        {
            var query = (DataServiceQuery)System.Linq.Queryable.Where(Context.CreateQuery<TInstance>(_path), whereExpression);

            var path = query.RequestUri.ToString().Substring(Context.BaseUri.ToString().TrimEnd('/').Length + 1);

            return path;
        }

        public QueryableSet(
            DataServiceQuery inner,
            DataServiceContextWrapper context,
            EntityBase entity,
            string path)
            : base(inner, context)
        {
            Initialize(inner, context, entity, path);
        }

        public void Initialize(DataServiceQuery inner,
            DataServiceContextWrapper context,
            EntityBase entity,
            string path)
        {
            base.Initialize(inner, context);
            _path = path;
            _entity = entity;
        }
    }
}