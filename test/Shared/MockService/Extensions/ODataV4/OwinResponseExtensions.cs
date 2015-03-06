using System;
using System.Linq;
using Microsoft.Owin;
using Newtonsoft.Json.Linq;

namespace Microsoft.MockService.Extensions.ODataV4
{
    public static class OwinResponseExtensions
    {
        public static IOwinResponse WithDefaultODataHeaders(this IOwinResponse owinResponse, string location = "http://localhost/")
        {
            owinResponse.Headers["location"] = location;
            owinResponse.Headers["Content-Type"] = "application/json;odata.metadata=minimal";
            owinResponse.Headers["OData-Version"] = "4.0";

            return owinResponse;
        }

        public static IOwinResponse WithODataEntityResponseBody(this IOwinResponse owinResponse, string baseAddress, 
            string entitySetName, object response, params JProperty[] additionalProperties)
        {
            if (response == null)
                return owinResponse;

            var jObject = JObject.FromObject(response);

            foreach (var additionalProperty in additionalProperties.Reverse())
            {
                jObject.AddFirst(additionalProperty);
            }

            jObject.AddOdataContext(baseAddress, entitySetName);

            owinResponse.Write(jObject.ToString());

            return owinResponse;
        }
    }
}