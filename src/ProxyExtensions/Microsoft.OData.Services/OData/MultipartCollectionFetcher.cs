using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.OData.Services.OData
{
    public class MultipartCollectionFetcher<TEntity, TFetcher, TOperations>:ODataCollectionFetcher<TEntity, TFetcher, TOperations>
        where TFetcher : ODataFetcher<TEntity>
        where TOperations : ODataOperations
    {
        public async Task Add(IList<MultipartElement> multiPartElements)
        {
            throw new NotImplementedException();
        }
    }
}
