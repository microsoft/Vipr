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

        internal static IOwinResponse WithODataEntityResponseBody(this IOwinResponse owinResponse, string baseAddress, 
            string entitySetName, object response)
        {
            if (response == null)
                return owinResponse;

            var jObject = JObject.FromObject(response);

            jObject.AddOdataContext(baseAddress, entitySetName);

            owinResponse.Write(jObject.ToString());

            return owinResponse;
        }
    }
}