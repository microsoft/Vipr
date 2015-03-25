using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Microsoft.MockService.Extensions.ODataV4
{
    public static class MockServiceExtensions
    {
        public static ResponseBuilder OnPostEntityRequest(this MockService mockService, string entityPath)
        {
            return mockService
                .OnRequest(c => c.Request.Method == "POST" && c.Request.Path.Value == entityPath);
        }

        public static ResponseBuilder OnInvokeMethodRequest(this MockService mockService, string httpMethod, string methodPath,
            TestReadableStringCollection uriArguments, JObject expectedBody)
        {
            uriArguments = uriArguments ?? new TestReadableStringCollection(new Dictionary<string, string[]>());

            return mockService
                .OnRequest(c => 
                    c.Request.Method == httpMethod &&
                    c.Request.Path.Value.StartsWith(methodPath) &&
                    c.Request.InvokesMethodWithParameters(methodPath, uriArguments) &&
                    ((c.Request.Body.Length == 0 && expectedBody == null) || (JToken.DeepEquals(expectedBody, c.Request.Body.ToJObject()))));
        }

        public static MockService RespondWithCreateEntity(this ResponseBuilder responseBuilder, string entitySetName, object response = null)
        {

            return responseBuilder.RespondWith((c, b) =>
            {
                c.Response.StatusCode = 201;
                c.Response.WithDefaultODataHeaders();
                c.Response.WithODataEntityResponseBody(b, entitySetName, response);
            });
        }

        public static MockService RespondWithGetEntity(this ResponseBuilder responseBuilder, string entitySetName, object response = null)
        {

            return responseBuilder.RespondWith((c, b) =>
            {
                c.Response.StatusCode = 200;
                c.Response.WithDefaultODataHeaders();
                c.Response.WithODataEntityResponseBody(b, entitySetName, response);
            });
        }
        
        public static MockService SetupPostEntityChanges(this MockService mockService, string entitySetPath)
        {
            mockService
                .OnRequest(c => c.Request.Method == "POST" && c.Request.Path.Value == entitySetPath)
                .RespondWith((c, b) =>
                {
                    c.Response.StatusCode = 200;
                    c.Response.WithDefaultODataHeaders();
                });

            return mockService;
        }

        public static MockService SetupPatchEntityChanges(this MockService mockService, string entitySetPath)
        {
            mockService
                .OnRequest(c => c.Request.Method == "PATCH" && c.Request.Path.Value == entitySetPath)
                .RespondWith((c, b) =>
                {
                    c.Response.StatusCode = 200;
                    c.Response.WithDefaultODataHeaders();
                });

            return mockService;
        }

        public static MockService SetupGetEntity(this MockService mockService, string entitySetPath,
            string entitySetName, object response, IEnumerable<string> expandTargets = null)
        {
            var jObject = JObject.FromObject(response);

            jObject.AddOdataContext(mockService.GetBaseAddress(), entitySetName);

            mockService
                .OnRequest(c => 
                    c.Request.Method == "GET" &&
                    c.Request.Path.Value == entitySetPath &&
                    (expandTargets == null || c.Request.Query["$expand"] == string.Join(",", expandTargets)))
                .RespondWith((c, b) =>
                    {
                        c.Response.StatusCode = 200;
                        c.Response.WithDefaultODataHeaders();
                        c.Response.Write(jObject.ToString());
                    });

            return mockService;
        }

        public static MockService SetupGetEntitySetCount(this MockService mockService, string entitySetPath,
            long count)
        {
            mockService
                .OnRequest(c => c.Request.Method == "GET" && c.Request.Path.Value == entitySetPath + "/$count")
                .RespondWith((c, b) =>
                {
                    c.Response.StatusCode = 200;
                    c.Response.WithDefaultODataHeaders();
                    c.Response.ContentType = "text/plain";
                    c.Response.Write(count.ToString());
                });

            return mockService;
        }

        public static MockService SetupGetWithEmptyResponse(this MockService mockService, string entityPropertyPath)
        {
            mockService
                .OnRequest(c => c.Request.Method == "GET" && c.Request.Path.Value == entityPropertyPath)
                .RespondWith((c, b) =>
                {
                    c.Response.StatusCode = 200;
                });

            return mockService;
        }

        // This method is not yet working
        // It needs to be able to set up odata context and related properties
        public static MockService SetupGetEntityProperty(this MockService mockService, string entityPropertyPath,
            string entitySetName, string entityKeyPredicate, string propertyName, object propertyValue)
        {
            var jObject = new JObject();

            if (propertyValue != null)
            {
                jObject.AddOdataContext(mockService.GetBaseAddress(), entitySetName, entityKeyPredicate, propertyName);
                jObject.Add(new JProperty("value", propertyValue));
            }

            mockService
                .OnRequest(c => c.Request.Method == "GET" && c.Request.Path.Value == entityPropertyPath)
                .RespondWith((c, b) =>
                {
                    c.Response.StatusCode = 200;
                    c.Response.WithDefaultODataHeaders();
                    if (propertyValue != null)
                        c.Response.Write(jObject.ToString());
                });

            return mockService;
        }
    }
}
