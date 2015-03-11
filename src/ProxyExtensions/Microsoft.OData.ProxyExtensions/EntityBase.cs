// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.OData.Client;

namespace Microsoft.OData.ProxyExtensions
{
    public class EntityBase : BaseEntityType, IEntityBase
    {
        private Lazy<HashSet<string>> _changedProperties = new Lazy<HashSet<string>>(true);

        public Lazy<HashSet<string>> ChangedProperties
        {
            get { return _changedProperties; }
        }

        protected Tuple<EntityBase, string> GetContainingEntity(string propertyName)
        {
            return new Tuple<EntityBase, string>(this, propertyName);
        }

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            _changedProperties.Value.Add(propertyName);
            if (Context != null)
            {
                Context.UpdateObject(this);
            }
        }

        public void ResetChanges()
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

        public void Initialize()
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

        /// <param name="deferSaveChanges">true to delay saving until batch is saved; false to save immediately.</param>
        public Task UpdateAsync(bool deferSaveChanges = false)
        {
            if (Context == null) throw new InvalidOperationException("Not Initialized");
            Context.UpdateObject(this);
            return SaveChangesAsync(deferSaveChanges);
        }

        /// <param name="deferSaveChanges">true to delay saving until batch is saved; false to save immediately.</param>
        public Task DeleteAsync(bool deferSaveChanges = false)
        {
            if (Context == null) throw new InvalidOperationException("Not Initialized");
            Context.DeleteObject(this);
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

        protected IReadOnlyQueryableSet<TInterface> CreateQuery<TInstance, TInterface>()
        {
            return new ReadOnlyQueryableSet<TInterface>(Context.CreateQuery<TInstance>(GetPath(null)), Context);
        }
    }
}
