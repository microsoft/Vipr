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
            get
            {
                return (DataServiceContextWrapper)base.Context;
            }
            private set
            {
                base.Context = value;
            }
        }

        public RestShallowObjectFetcher() { }

        public void Initialize(
            DataServiceContextWrapper context,
            string path)
        {
            Context = context;
            _path = path;
            _isInitialized = true;
        }

        protected string GetPath(string propertyName)
        {
            ThrowIfNotInitialized();

            return propertyName == null ? this._path : this._path + "/" + propertyName;
        }

        protected System.Uri GetUrl()
        {
            ThrowIfNotInitialized();

            return new Uri(Context.BaseUri.ToString().TrimEnd('/') + "/" + GetPath(null));
        }

        protected IReadOnlyQueryableSet<TIInstance> CreateQuery<TInstance, TIInstance>()
            where TInstance : EntityBase, TIInstance
        {
            ThrowIfNotInitialized();

            return new ReadOnlyQueryableSet<TIInstance>(this.Context.CreateQuery<TInstance>(this.GetPath(null)), this.Context);
        }

        private void ThrowIfNotInitialized()
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Initialize must be called before invoking this operation.");
        }
    }
}