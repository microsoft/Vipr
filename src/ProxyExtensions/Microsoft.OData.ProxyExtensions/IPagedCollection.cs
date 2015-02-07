namespace Microsoft.OData.ProxyExtensions
{
    public interface IPagedCollection
    {
        global::System.Collections.Generic.IReadOnlyList<object> CurrentPage { get; }
        bool MorePagesAvailable { get; }
        global::System.Threading.Tasks.Task<IPagedCollection> GetNextPageAsync();
    }

    public interface IPagedCollection<TElement>
    {
        global::System.Collections.Generic.IReadOnlyList<TElement> CurrentPage { get; }
        bool MorePagesAvailable { get; }
        global::System.Threading.Tasks.Task<IPagedCollection<TElement>> GetNextPageAsync();
    }
}