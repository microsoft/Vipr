// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.OData.Client;

namespace Microsoft.OData.ProxyExtensions
{
    public class RestShallowObjectFetcher : BaseEntityType
    {
        private string _path;
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
    }
}
