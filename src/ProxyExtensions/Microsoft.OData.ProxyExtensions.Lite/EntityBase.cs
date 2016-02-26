// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.OData.Client;

namespace Microsoft.OData.ProxyExtensions.Lite
{
    public class EntityBase : BaseEntityType
    {
        private Lazy<HashSet<string>> _changedProperties = new Lazy<HashSet<string>>(true);

        protected internal Lazy<HashSet<string>> ChangedProperties
        {
            get { return _changedProperties; }
        }

        protected Tuple<EntityBase, string> GetContainingEntity(string propertyName)
        {
            return new Tuple<EntityBase, string>(this, propertyName);
        }

        protected internal virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            _changedProperties.Value.Add(propertyName);
        }

        protected internal void ResetChanges()
        {
            _changedProperties = new Lazy<HashSet<string>>(true);
        }

        protected internal new DataServiceContextWrapper Context
        {
            get
            {
                return (DataServiceContextWrapper)base.Context;
            }
            private set
            {
                base.Context = value;
            }
        }

        protected internal void Initialize()
        {
        }

        protected string GetPath(string propertyName)
        {
            Uri uri = GetUrl();
            if (uri != null)
            {
                return uri.ToString().Substring(Context.BaseUri.ToString().Length) + "/" + propertyName;
            }

            return null;
        }

        protected Uri GetUrl()
        {
            if (Context == null)
            {
                return null;
            }

            Uri uri;
            Context.TryGetUri(this, out uri);

            return uri;
        }

        protected IReadOnlyQueryableSet<TInterface> CreateQuery<TInstance, TInterface>()
        {
            return new ReadOnlyQueryableSet<TInterface>(Context.CreateQuery<TInstance>(GetPath(null)), Context);
        }
    }
}
