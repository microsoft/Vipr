using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.OData.Services.Interfaces;

namespace Microsoft.OData.Services.OData
{
    public class ODataContainer: ODataExecutable
    {
        public string BaseUri { get; set; }

        protected override async Task<HttpResponseMessage> Execute(HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }

        protected override IDependencyResolver Resolver
        {
            get { throw new NotImplementedException(); }
        }

        public static string GenerateParametersPayload(IDictionary<string, object> parameters, IDependencyResolver resolver)
        {
            return resolver.JsonSerializer.Serialize(parameters);
        }
    }
}
