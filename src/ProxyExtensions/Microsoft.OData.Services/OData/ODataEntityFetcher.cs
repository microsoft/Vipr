using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.OData.Services.Interfaces;

namespace Microsoft.OData.Services.OData
{
    public class ODataEntityFetcher<TEntity, TOperations> : ODataFetcher<TEntity>, IReadable<TEntity>
        where TOperations: ODataOperations
    {
        // THIS CLASS SHOULD HANDLE SELECT AND EXPAND VIA LINQ.

        protected override Task<HttpResponseMessage> Execute(HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> Update(TEntity updatedEntity)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UpdateRaw(string payload)
        {
            throw new NotImplementedException();
        }

        public async Task Delete()
        {
            throw new NotImplementedException();
        }

        public override async Task<TEntity> Read()
        {
            throw new NotImplementedException();
        }

        public override async Task<string> ReadRaw()
        {
            throw new NotImplementedException();
        }

        public TOperations GetOperations()
        {
            throw new NotImplementedException();
        }
    }
}
