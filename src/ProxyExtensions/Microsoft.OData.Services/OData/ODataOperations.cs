using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.OData.Services.OData
{
    public class ODataOperations : ODataExecutable
    {
        protected override Task<HttpResponseMessage> Execute(HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }

        protected override Services.Interfaces.IDependencyResolver Resolver
        {
            get { throw new NotImplementedException(); }
        }
    }
}
