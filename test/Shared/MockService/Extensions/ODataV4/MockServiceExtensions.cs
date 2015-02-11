using System;
using System.Collections.Generic;
using Microsoft.Owin;
using Newtonsoft.Json.Linq;
using Microsoft.MockService;

namespace Microsoft.MockService.Extensions.ODataV4
{
    public static class MockServiceExtensions
    {
        public static MockService SetupPostEntity(this MockService mockService, string entityPath,
            string entitySetName, object response = null)
        {
            mockService
                .Setup(c => c.Request.Method == "POST" &&
                            c.Request.Path.Value == entityPath,
                    (b, c) =>
                    {
                        c.Response.StatusCode = 201;
                        c.Response.WithDefaultODataHeaders();
                        c.Response.WithODataEntityResponseBody(mockService.GetBaseAddress(), entitySetName, response);
                    });

            return mockService;
        }

        public static MockService SetupMethod(this MockService mockService, string httpMethod, string methodPath,
            TestReadableStringCollection uriArguments, JObject expectedBody, string entitySetName, object response = null)
        {
            uriArguments = uriArguments ?? new TestReadableStringCollection(new Dictionary<string, string[]>());

            mockService
                .Setup(c => c.Request.Method == httpMethod &&
                            c.Request.Path.Value.StartsWith(methodPath) &&
                            c.Request.InvokesMethodWithParameters(methodPath, uriArguments) &&
                            ((c.Request.Body.Length == 0 && expectedBody == null) || (JToken.DeepEquals(expectedBody, c.Request.Body.ToJObject()))),
                    (b, c) =>
                    {
                        c.Response.StatusCode = 200;
                        if (response != null)
                        {
                            c.Response.WithDefaultODataHeaders();
                            c.Response.WithODataEntityResponseBody(mockService.GetBaseAddress(), entitySetName, response);
                        }
                    });

            return mockService;
        }

        public static MockService SetupMethod(this MockService mockService, string httpMethod, string methodPath,
            TestReadableStringCollection uriArguments, JObject expectedBody, JObject response = null)
        {
            uriArguments = uriArguments ?? new TestReadableStringCollection(new Dictionary<string, string[]>());

            mockService
                .Setup(c => c.Request.Method == httpMethod &&
                            c.Request.Path.Value.StartsWith(methodPath) &&
                            c.Request.InvokesMethodWithParameters(methodPath, uriArguments) &&
                            ((c.Request.Body.Length == 0 && expectedBody == null) || (JToken.DeepEquals(expectedBody, c.Request.Body.ToJObject()))),
                    (b, c) =>
                    {
                        c.Response.StatusCode = 200;
                        if (response != null)
                        {
                            c.Response.WithDefaultODataHeaders();
                            c.Response.Write(response.ToString());
                        }
                    });

            return mockService;
        }
        
        public static MockService SetupPostEntityChanges(this MockService mockService, string entitySetPath)
        {
            mockService
                .Setup(c => c.Request.Method == "POST" &&
                            c.Request.Path.Value == entitySetPath,
                    (b, c) =>
                    {
                        c.Response.StatusCode = 200;
                        c.Response.WithDefaultODataHeaders();
                    });

            return mockService;
        }

        public static MockService SetupPatchEntityChanges(this MockService mockService, string entitySetPath)
        {
            mockService
                .Setup(c => c.Request.Method == "PATCH" &&
                            c.Request.Path.Value == entitySetPath,
                    (b, c) =>
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
                .Setup(c => c.Request.Method == "GET" &&
                            c.Request.Path.Value == entitySetPath &&
                            (expandTargets == null || c.Request.Query["$expand"] == string.Join(",", expandTargets)),
                    (b, c) =>
                    {
                        c.Response.StatusCode = 200;
                        c.Response.WithDefaultODataHeaders();
                        c.Response.Write(jObject.ToString());
                    });

            return mockService;
        }

        public static MockService SetupGetWithEmptyResponse(this MockService mockService, string entityPropertyPath)
        {
            mockService
                .Setup(c => c.Request.Method == "GET" &&
                            c.Request.Path.Value == entityPropertyPath,
                    (b, c) =>
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
                .Setup(c => c.Request.Method == "GET" &&
                            c.Request.Path.Value == entityPropertyPath,
                    (b, c) =>
                    {
                        c.Response.StatusCode = 200;
                        c.Response.WithDefaultODataHeaders();
                        if(propertyValue != null)
                            c.Response.Write(jObject.ToString());
                    });

            return mockService;
        }
    }
}
