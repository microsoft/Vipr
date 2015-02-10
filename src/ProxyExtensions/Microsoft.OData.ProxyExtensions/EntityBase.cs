using System;
using System.Collections.Generic;
using Microsoft.OData.Client;

namespace Microsoft.OData.ProxyExtensions
{
    public class EntityBase : BaseEntityType
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

        public virtual void OnPropertyChanged([global::System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
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

        protected System.Uri GetUrl()
        {
            if (Context == null)
            {
                return null;
            }

            Uri uri;
            Context.TryGetUri(this, out uri);

            return uri;
        }

        /// <param name=""dontSave"">true to delay saving until batch is saved; false to save immediately.</param>
        public global::System.Threading.Tasks.Task UpdateAsync(bool dontSave = false)
        {
            if (Context == null) throw new InvalidOperationException("Not Initialized");
            Context.UpdateObject(this);
            return SaveAsNeeded(dontSave);
        }

        /// <param name=""dontSave"">true to delay saving until batch is saved; false to save immediately.</param>
        public global::System.Threading.Tasks.Task DeleteAsync(bool dontSave = false)
        {
            if (Context == null) throw new InvalidOperationException("Not Initialized");
            Context.DeleteObject(this);
            return SaveAsNeeded(dontSave);
        }

        /// <param name=""dontSave"">true to delay saving until batch is saved; false to save immediately.</param>
        public global::System.Threading.Tasks.Task SaveAsNeeded(bool dontSave)
        {
            if (!dontSave)
            {
                return Context.SaveChangesAsync();
            }
            else
            {
                var retVal = new global::System.Threading.Tasks.TaskCompletionSource<object>();
                retVal.SetResult(null);
                return retVal.Task;
            }
        }

        protected IReadOnlyQueryableSet<TInterface> CreateQuery<TInstance, TInterface>()
        {
            return new ReadOnlyQueryableSet<TInterface>(Context.CreateQuery<TInstance>(GetPath(null)), Context);
        }
    }
}