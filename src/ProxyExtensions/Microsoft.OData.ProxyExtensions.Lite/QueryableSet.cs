// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq.Expressions;
using Microsoft.OData.Client;
using System.Threading.Tasks;

namespace Microsoft.OData.ProxyExtensions.Lite
{
    public class QueryableSet<TSource> : ReadOnlyQueryableSetBase<TSource>
    {
        protected string Path;
        protected object Entity;

        public void SetContainer(Func<EntityBase> entity, string property)
        {
            // Unneeded
        }

        /// <param name="deferSaveChanges">true to delay saving until batch is saved; false to save immediately.</param>
        protected Task AddAsync<T>(T item, bool deferSaveChanges = false)
        {
            if (Context == null) throw new InvalidOperationException("Not Initialized");

            if (Entity == null)
            {
                Context.AddObject(Path, item);
            }
            else
            {
                var lastSlash = Path.LastIndexOf('/');
                var shortPath = (lastSlash >= 0 && lastSlash < Path.Length - 1) ? Path.Substring(lastSlash + 1) : Path;
                Context.AddRelatedObject(Entity, shortPath, item);
            }

            return SaveChangesAsync(deferSaveChanges);
        }

        /// <param name="deferSaveChanges">true to delay saving until batch is saved; false to save immediately.</param>
        protected Task UpdateAsync<T>(T item, bool deferSaveChanges = false)
        {
            if (Context == null) throw new InvalidOperationException("Not Initialized");
            Context.UpdateObject(item as EntityBase);
            return SaveChangesAsync(deferSaveChanges);
        }

        /// <param name="deferSaveChanges">true to delay saving until batch is saved; false to save immediately.</param>
        protected Task DeleteAsync<T>(T item, bool deferSaveChanges = false)
        {
            if (Context == null) throw new InvalidOperationException("Not Initialized");
            Context.DeleteObject(item);
            return SaveChangesAsync(deferSaveChanges);
        }

        protected Task AddLinkAsync<T>(object source, T target, bool deferSaveChanges = false)
        {
            if (Context == null) throw new InvalidOperationException("Not Initialized");
            Context.AddLink(source, GetProperty(), target);
            return SaveChangesAsync(deferSaveChanges);
        }

        protected Task RemoveLinkAsync<T>(object source, T target, bool deferSaveChanges = false)
        {
            if (Context == null) throw new InvalidOperationException("Not Initialized");
            Context.DeleteLink(source, GetProperty(), target);
            return SaveChangesAsync(deferSaveChanges);
        }

        /// <param name="deferSaveChanges">true to delay saving until batch is saved; false to save immediately.</param>
        /// <param name="saveChangesOption">Save changes option to control how change requests are sent to the service.</param>
        public Task SaveChangesAsync(bool deferSaveChanges = false, SaveChangesOptions saveChangesOption = SaveChangesOptions.None)
        {
            if (deferSaveChanges)
            {
                var retVal = new TaskCompletionSource<object>();
                retVal.SetResult(null);
                return retVal.Task;
            }

            return Context.SaveChangesAsync(saveChangesOption);
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

        private string GetProperty()
        {
            Path = Path.TrimEnd(new char[] { '/' });
            var property = Path.Substring(Path.LastIndexOf('/') + 1);
            return property;
        }
    }
}
