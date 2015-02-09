using System.Threading.Tasks;
using Microsoft.OData.ProxyExtensions;

namespace CSharpWriterUnitTests
{
    public static class ProxyExtensionExtensions
    {
        public static Task ExecuteAsync(this ReadOnlyQueryableSetBase collection)
        {
            return collection.InvokeMethod<Task>("ExecuteAsync", args: new object[0]);
        }

        public static Task ExecuteAsync(this RestShallowObjectFetcher fetcher)
        {
            return fetcher.InvokeMethod<Task>("ExecuteAsync", args: new object[0]);
        }
        
    }
}