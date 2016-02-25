// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.OData.Client;

namespace Microsoft.OData.ProxyExtensions.Lite
{
    public class RestShallowObjectFetcher : BaseEntityType
    {
        private string _path;
        private string _propertyName;
        private bool _isInitialized;

        public new DataServiceContextWrapper Context
        {
            get { return (DataServiceContextWrapper) base.Context; }
            private set { base.Context = value; }
        }

        public void Initialize(DataServiceContextWrapper context, string path)
        {
            Context = context;
            _path = path;
            _isInitialized = true;
            _propertyName = GetProperty();
        }

        /// <param name="deferSaveChanges">true to delay saving until batch is saved; false to save immediately.</param>
        protected Task UpdateAsync<T>(T item, bool deferSaveChanges = false)
        {
            if (Context == null) throw new InvalidOperationException("Not Initialized");
            Context.UpdateObject(item as EntityBase);
            return FetcherSaveChangesAsync(deferSaveChanges);
        }

        /// <param name="deferSaveChanges">true to delay saving until batch is saved; false to save immediately.</param>
        protected Task DeleteAsync<T>(T item, bool deferSaveChanges = false)
        {
            if (Context == null) throw new InvalidOperationException("Not Initialized");
            Context.DeleteObject(item);
            return FetcherSaveChangesAsync(deferSaveChanges);
        }

        /// <param name="deferSaveChanges">true to delay saving until batch is saved; false to save immediately.</param>
        protected Task SetAsync<T>(object source, T target, bool deferSaveChanges = false)
        {
            ThrowIfNotInitialized();
            Context.UpdateRelatedObject(source, _propertyName, target);
            return FetcherSaveChangesAsync(deferSaveChanges);
        }

        /// <param name="deferSaveChanges">true to delay saving until batch is saved; false to save immediately.</param>
        protected Task FetcherDeleteLinkAsync(object source, bool deferSaveChanges = false)
        {
            ThrowIfNotInitialized();
            Context.SetLink(source, _propertyName, null);
            return FetcherSaveChangesAsync(deferSaveChanges);
        }

        /// <param name="deferSaveChanges">true to delay saving until batch is saved; false to save immediately.</param>
        protected Task UpdateLinkAsync<T>(object source, T target, bool deferSaveChanges = false)
        {
            ThrowIfNotInitialized();
            Context.SetLink(source, _propertyName, target);
            return FetcherSaveChangesAsync(deferSaveChanges);
        }

        /// <param name="deferSaveChanges">true to delay saving until batch is saved; false to save immediately.</param>
        /// <param name="saveChangesOption">Save changes option to control how change requests are sent to the service.</param>
        protected Task FetcherSaveChangesAsync(bool deferSaveChanges = false, SaveChangesOptions saveChangesOption = SaveChangesOptions.None)
        {
            if (deferSaveChanges)
            {
                var retVal = new TaskCompletionSource<object>();
                retVal.SetResult(null);
                return retVal.Task;
            }

            return Context.SaveChangesAsync(saveChangesOption);
        }

        protected string GetPath(string propertyName)
        {
            ThrowIfNotInitialized();

            return propertyName == null ? _path : _path + "/" + propertyName;
        }

        protected Uri GetUrl()
        {
            ThrowIfNotInitialized();

            return new Uri(Context.BaseUri.ToString().TrimEnd('/') + "/" + GetPath(null));
        }

        protected IReadOnlyQueryableSet<TIInstance> CreateQuery<TInstance, TIInstance>()
            where TInstance : EntityBase, TIInstance
        {
            ThrowIfNotInitialized();

            return new ReadOnlyQueryableSet<TIInstance>(Context.CreateQuery<TInstance>(GetPath(null)), Context);
        }

        private void ThrowIfNotInitialized()
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Initialize must be called before invoking this operation.");
        }

        private string GetProperty()
        {
            var path = GetPath(null);
            path = path.TrimEnd(new char[] {'/'});
            var property = path.Substring(path.LastIndexOf('/') + 1);
            return property;
        }
    }
}
