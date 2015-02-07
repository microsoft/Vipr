namespace Microsoft.OData.ProxyExtensions
{
    public interface IReadOnlyQueryableSet<TSource> : IReadOnlyQueryableSetBase<TSource>
    {
        System.Threading.Tasks.Task<IPagedCollection<TSource>> ExecuteAsync();
        System.Threading.Tasks.Task<TSource> ExecuteSingleAsync();
    }
}