using System;
using System.Threading.Tasks;
using Microsoft.OData.Services.Interfaces;

namespace Microsoft.OData.Services.OData
{
    public class ODataCollectionFetcher<TEntity, TFetcher, TOperations> : ODataFetcher<TEntity>, IReadable<TEntity>
        where TFetcher : ODataFetcher<TEntity>
        where TOperations : ODataOperations
    {
        // THIS CLASS SHOULD SUPPORT THE MORAL EQUIVALENT OF THE LINQ PROCESSING STUFF.

        public TFetcher GetById(string id)
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

        public async Task<TEntity> Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<string> AddRaw(string payload)
        {
            throw new NotImplementedException();
        }

        public ODataCollectionFetcher<TEntity, TFetcher, TOperations> AddParameter(string name, object value)
        {
            CustomParameters.Add(name, value);
            return this;
        }

        public ODataCollectionFetcher<TEntity, TFetcher, TOperations> AddHeader(string name, string value)
        {
            CustomHeaders.Add(name, value);
            return this;
        }

        public TOperations GetOperations()
        {
            throw new NotImplementedException();
        }
    }
}
