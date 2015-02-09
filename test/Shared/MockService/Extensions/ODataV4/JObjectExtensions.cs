using Newtonsoft.Json.Linq;

namespace Microsoft.MockService.Extensions.ODataV4
{
    public static class JObjectExtensions
    {
        public static JObject AddOdataContext(this JObject jObject, string baseUri, string entitySetName)
        {
            jObject.AddFirst(new JProperty("@odata.context", baseUri + "$metadata#" + entitySetName + "/$entity"));

            return jObject;
        }

        public static JObject AddOdataContext(this JObject jObject, string baseUri, string entitySetName,
            string entityKeyPredicate, string propertyName)
        {
            jObject.AddFirst(new JProperty("@odata.context", string.Format("{0}$metadata#{1}({2})/{3}", baseUri, entitySetName, entityKeyPredicate, propertyName)));

            return jObject;
        }
    }
}