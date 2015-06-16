using Microsoft.OData.Services.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.OData.Services.OData
{
    public abstract class ODataExecutable
    {
        protected Dictionary<string, object> CustomParameters { get; private set; }

        protected Dictionary<string, string> CustomHeaders { get; private set; }

        protected ODataExecutable()
        {
            CustomParameters = new Dictionary<string, object>();
            CustomHeaders = new Dictionary<string, string>();
        }

        protected abstract Task<HttpResponseMessage> Execute(HttpRequestMessage request);

        protected abstract IDependencyResolver Resolver { get; }

        protected void Log(string content, LogLevel logLevel)
        {
            Resolver.Logger.Log(content, logLevel);
        }
    }
}
