using Microsoft.Its.Recipes;
using Microsoft.Owin;

namespace CSharpWriterUnitTests
{
    public static class OwinResponseExtensions
    {
        public static void WithDefaultODataHeaders(this IOwinResponse r)
        {
            r.Headers["location"] = Any.Uri().AbsoluteUri;
            r.Headers["Content-Type"] = "application/json;odata.metadata=minimal";
            r.Headers["OData-Version"] = "4.0";
        }
    }
}