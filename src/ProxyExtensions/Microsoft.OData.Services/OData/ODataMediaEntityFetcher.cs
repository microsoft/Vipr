namespace Microsoft.OData.Services.OData
{
    public class ODataMediaEntityFetcher<TEntity, TFetcher, TOperations> : ODataEntityFetcher<TEntity, TOperations>
        where TFetcher : ODataFetcher<TEntity>
        where TOperations : ODataOperations
    {
    }
}
