using Microsoft.OData.Client;

namespace Microsoft.OData.ProxyExtensions
{
    public class ReadOnlyQueryableSet<TSource> : ReadOnlyQueryableSetBase<TSource>, IReadOnlyQueryableSet<TSource>
    {
        public ReadOnlyQueryableSet(
            DataServiceQuery inner,
            DataServiceContextWrapper context)
            : base(inner, context)
        {
        }


        public global::System.Threading.Tasks.Task<IPagedCollection<TSource>> ExecuteAsync()
        {
            return base.ExecuteAsyncInternal();
        }

        public global::System.Threading.Tasks.Task<TSource> ExecuteSingleAsync()
        {
            return base.ExecuteSingleAsyncInternal();
        }
    }
}