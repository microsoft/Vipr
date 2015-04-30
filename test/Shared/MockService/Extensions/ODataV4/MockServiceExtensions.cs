using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Microsoft.MockService.Extensions.ODataV4
{
    public static class MockServiceExtensions
    {
        public static ResponseBuilder OnPostEntityRequest(this MockService mockService, string entitySetPath)
        {
            return mockService
                .OnRequest(c => c.Request.Method == "POST" && c.Request.Path.Value == entitySetPath);
        }

        public static ResponseBuilder OnPutUpdateLinkRequest(this MockService mockService, string entitySetPath, JObject expectedBody)
        {
            return mockService
                .OnRequest(c => c.Request.Method == "PUT" && 
                    c.Request.Path.Value == entitySetPath + "/$ref" &&
                    ((c.Request.Body.Length == 0 && expectedBody == null) ||
                     (JToken.DeepEquals(expectedBody, c.Request.Body.ToJObject()))));
        }

        public static ResponseBuilder OnPatchEntityRequest(this MockService mockService, string entityPath, JObject expectedBody = null)
        {
            return mockService
                .OnRequest(c => c.Request.Method == "PATCH" && 
                    c.Request.Path.Value == entityPath &&
                    (expectedBody == null ||
                     (JToken.DeepEquals(expectedBody, c.Request.Body.ToJObject()))));
        }

        public static ResponseBuilder OnGetEntityRequest(this MockService mockService, string entitySetPath)
        {
            return mockService
                .OnRequest(c => c.Request.Method == "GET" && c.Request.Path.Value == entitySetPath);
        }

        public static ResponseBuilder OnDeleteEntityRequest(this MockService mockService, string entityPath)
        {
            return mockService
                .OnRequest(c => c.Request.Method == "DELETE" && c.Request.Path.Value == entityPath);
        }

        public static ResponseBuilder OnDeleteLinkRequest(this MockService mockService, string entityPath)
        {
            return mockService
                .OnRequest(c => c.Request.Method == "DELETE" && c.Request.Path.Value == entityPath + "/$ref");
        }

        public static ResponseBuilder OnGetEntityWithExpandRequest(this MockService mockService, string entitySetPath,
            IEnumerable<string> expandTargets)
        {
            return mockService
                .OnRequest(c =>
                    c.Request.Method == "GET" &&
                    c.Request.Path.Value == entitySetPath &&
                    (c.Request.Query["$expand"] == string.Join(",", expandTargets)));
        }

        public static ResponseBuilder OnGetEntityCountRequest(this MockService mockService, string entitySetPath)
        {
            return mockService
                .OnRequest(c => c.Request.Method == "GET" && c.Request.Path.Value == entitySetPath + "/$count");
        }

        public static ResponseBuilder OnGetEntityPropertyRequest(this MockService mockService, string entityPath,
            string propertyName)
        {
            return mockService
                .OnRequest(c => c.Request.Method == "GET" && c.Request.Path.Value == entityPath + "/" + propertyName);
        }

        public static ResponseBuilder OnPostEntityPropertyRequest(this MockService mockService, string entityPath,
            string propertyName)
        {
            return mockService
                .OnRequest(c => c.Request.Method == "POST" && c.Request.Path.Value == entityPath + "/" + propertyName);
        }

        public static ResponseBuilder OnInvokeMethodRequest(this MockService mockService, string httpMethod,
            string methodPath,
            TestReadableStringCollection uriArguments, JObject expectedBody)
        {
            uriArguments = uriArguments ?? new TestReadableStringCollection(new Dictionary<string, string[]>());

            return mockService
                .OnRequest(c =>
                    c.Request.Method == httpMethod &&
                    c.Request.Path.Value.StartsWith(methodPath) &&
                    c.Request.InvokesMethodWithParameters(methodPath, uriArguments) &&
                    ((c.Request.Body.Length == 0 && expectedBody == null) ||
                     (JToken.DeepEquals(expectedBody, c.Request.Body.ToJObject()))));
        }
    }
}
