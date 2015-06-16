using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.OData.Services.Interfaces;

namespace Microsoft.OData.Services.OData
{
    public class ODataFetcher<TEntity> : ODataExecutable, IReadable<TEntity>
    {
        protected override Task<HttpResponseMessage> Execute(HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }

        protected override IDependencyResolver Resolver
        {
            get { throw new NotImplementedException(); }
        }

        public virtual async Task<string> ReadRaw()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TEntity> Read()
        {
            throw new NotImplementedException();
        }
    }
}
