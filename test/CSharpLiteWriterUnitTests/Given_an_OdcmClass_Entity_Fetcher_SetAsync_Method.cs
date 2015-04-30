
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Its.Recipes;
using Microsoft.MockService;
using Microsoft.MockService.Extensions.ODataV4;
using Microsoft.OData.ProxyExtensions.Lite;
using Newtonsoft.Json.Linq;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpLiteWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Fetcher_SetAsync_Method : EntityTestBase
    {
        private MockService _mockedService;
        private OdcmEntityClass _navPropertyClass;
        private OdcmProperty _navProperty;
        private System.Type _navPropertyFetcherType;
        private System.Type _navPropertyConcreteType;

        public Given_an_OdcmClass_Entity_Fetcher_SetAsync_Method()
        {
            Init(odcmModel =>
            {
                // create a single-valued navigation property for 'Class' entity type.
                _navPropertyClass = Any.OdcmEntityClass(Namespace);
                odcmModel.AddType(_navPropertyClass);
                _navProperty = Any.OdcmProperty(p =>
                {
                    p.Class = Class;
                    p.Projection = new OdcmProjection()
                    {
                        Type = _navPropertyClass
                    };
                });
                Class.Properties.Add(_navProperty);
            });

            _navPropertyFetcherType = Proxy.GetClass(_navPropertyClass.Namespace, _navPropertyClass.Name + "Fetcher");
            _navPropertyConcreteType = Proxy.GetClass(_navPropertyClass.Namespace, _navPropertyClass.Name);
        }

        /*
         * PATCH request can be used push entity objects created on the client to the server.
         * In this test 'SetAsync' is called to create a new entity on the server.
         * Example request
         * PATCH http://services.odata.org/V4/TripPinServiceRW/People('russellwhyte')/Photo
         * Request Body 
         * *Serliazed JSON object of the newly created 'Photo' entity*
         * 
         * This request creates a new 'Photo' for  entity "People('russellwhyte')" at its single-valued navigation 
         * property called 'Photo'.
         * 
         * Spec - http://docs.oasis-open.org/odata/odata/v4.0/errata02/os/complete/part1-protocol/odata-v4.0-errata02-os-part1-protocol-complete.html#_Toc406398330
         */

        [Fact]
        public void It_creates_a_new_entity_for_singlevalued_navigation_property()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var propertyPath = Class.GetDefaultEntityPath(entityKeyValues) + "/" + _navProperty.Name;
            var entity2KeyValues = _navPropertyClass.GetSampleKeyArguments().ToArray();
            var expectedJObject = new JObject();
            var targetInstance = Activator.CreateInstance(_navPropertyConcreteType);

            foreach (var keyProperty in entity2KeyValues)
            {
                var key = _navPropertyConcreteType.GetKeyProperties().Single(p => p.Name == keyProperty.Item1);
                key.SetValue(targetInstance, keyProperty.Item2);
                expectedJObject[keyProperty.Item1] = (string)keyProperty.Item2;
            }

            using (_mockedService = new MockService()
                    .SetupPostEntity(TargetEntity, entityKeyValues)
                    .OnPatchEntityRequest(propertyPath, expectedJObject)
                    .RespondWithODataOk())
            {
                var context = _mockedService.GetDefaultContext(Model);
                var fetcher = context.CreateFetcher(_navPropertyFetcherType, propertyPath);
                var sourceInstance = context.CreateConcrete(ConcreteType);

                fetcher.InvokeMethod<Task>("SetAsync", new object[] { sourceInstance, targetInstance, System.Type.Missing }).Wait();
            }
        }

        [Fact]
        public void It_does_not_create_a_new_entity_when_delay_saving()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var propertyPath = Class.GetDefaultEntityPath(entityKeyValues) + "/" + _navProperty.Name;
            var targetInstance = Activator.CreateInstance(_navPropertyConcreteType);

            using (_mockedService = new MockService()
                    .SetupPostEntity(TargetEntity, entityKeyValues))
            {
                var context = _mockedService.GetDefaultContext(Model);
                var fetcher = context.CreateFetcher(_navPropertyFetcherType, propertyPath);
                var sourceInstance = context.CreateConcrete(ConcreteType);

                fetcher.InvokeMethod<Task>("SetAsync", new object[] { sourceInstance, targetInstance, true }).Wait();
            }
        }
    }
}
