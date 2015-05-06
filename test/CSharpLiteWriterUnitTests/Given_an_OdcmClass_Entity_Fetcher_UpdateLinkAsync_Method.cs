
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Its.Recipes;
using Microsoft.MockService;
using Microsoft.MockService.Extensions.ODataV4;
using Newtonsoft.Json.Linq;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpLiteWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Fetcher_UpdateLinkAsync_Method : NavigationPropertyTestBase
    {
        private MockService _mockedService;

        public Given_an_OdcmClass_Entity_Fetcher_UpdateLinkAsync_Method()
        {
            Init(odcmModel =>
            {
                // create a single-valued navigation property for 'Class' entity type.
                NavTargetClass = Any.OdcmEntityClass(Namespace);
                odcmModel.AddType(NavTargetClass);
                NavigationProperty = Any.OdcmProperty(p =>
                {
                    p.Class = Class;
                    p.Projection = new OdcmProjection()
                    {
                        Type = NavTargetClass
                    };
                });
                Class.Properties.Add(NavigationProperty);

                var serviceClass = Model.EntityContainer;

                var projection = new OdcmProjection() { Type = NavTargetClass };

                serviceClass.Properties.Add(new OdcmProperty(NavTargetClass.Name) { Class = serviceClass, Projection = projection });

                serviceClass.Properties.Add(new OdcmProperty(NavTargetClass.Name + "s")
                {
                    Class = serviceClass,
                    Projection = projection,
                    IsCollection = true
                });
            });
        }

        /*
         * PUT request can be used to modify relationships/links between entities.
         * In this test 'UpdateLinkAsync' is called to update a link between an entitytype and its 
         * single-valued navigation property.
         * Example request
         * PUT http://services.odata.org/V4/TripPinServiceRW/People('russellwhyte')/Photo/$ref
         * Request Body
         * {"@odata.id":"http://services.odata.org/V4/TripPinServiceRW/Photos(2)"}
         * 
         * This request modifies the link between entity "People('russellwhyte')" and its single-valued navigation 
         * property called 'Photo' to point to Photos(2).
         * 
         * Spec - http://docs.oasis-open.org/odata/odata/v4.0/errata02/os/complete/part1-protocol/odata-v4.0-errata02-os-part1-protocol-complete.html#_Toc406398335
         */

        [Fact]
        public void It_Updates_the_link_between_entity_and_singlevalued_navigation_property()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var propertyPath = Class.GetDefaultEntityPath(entityKeyValues) + "/" + NavigationProperty.Name;
            var relatedEntityKeyValues = NavTargetClass.GetSampleKeyArguments().ToArray();
            var relatedEntityPath = NavTargetClass.GetDefaultEntityPath(relatedEntityKeyValues);

            using (_mockedService = new MockService())
            {
                var baseAddress = _mockedService.GetBaseAddress().TrimEnd('/');
                var expectedJObject = new JObject();
                expectedJObject["@odata.id"] = baseAddress + relatedEntityPath;

                _mockedService
                    .SetupPostEntity(TargetEntity, entityKeyValues)
                    .SetupPostEntity(NavTargetEntity, relatedEntityKeyValues)
                    .OnPutUpdateLinkRequest(propertyPath, expectedJObject)
                    .RespondWithODataOk();
            
                var context = _mockedService.GetDefaultContext(Model);
                var fetcher = context.CreateFetcher(NavTargetFetcherType, propertyPath);
                var sourceInstance = context.CreateConcrete(ConcreteType);
                var targetInstance = context.CreateConcrete(NavTargetConcreteType);

                fetcher.InvokeMethod<Task>("UpdateLinkAsync", new object[] { sourceInstance, targetInstance, System.Type.Missing }).Wait();
            }
        }

        [Fact]
        public void It_does_not_update_a_link_when_delay_saving()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var propertyPath = Class.GetDefaultEntityPath(entityKeyValues) + "/" + NavigationProperty.Name;
            var relatedEntityKeyValues = NavTargetClass.GetSampleKeyArguments().ToArray();

            using (_mockedService = new MockService()
                    .SetupPostEntity(TargetEntity, entityKeyValues)
                    .SetupPostEntity(NavTargetEntity, relatedEntityKeyValues))
            {

                var context = _mockedService.GetDefaultContext(Model);
                var fetcher = context.CreateFetcher(NavTargetFetcherType, propertyPath);
                var sourceInstance = context.CreateConcrete(ConcreteType);
                var targetInstance = context.CreateConcrete(NavTargetConcreteType);

                fetcher.InvokeMethod<Task>("UpdateLinkAsync", new object[] { sourceInstance, targetInstance, true }).Wait();
            }
        }

        [Fact]
        public void It_updates_a_link_when_delay_saving_and_calling_SaveChangesAsync()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var propertyPath = Class.GetDefaultEntityPath(entityKeyValues) + "/" + NavigationProperty.Name;
            var relatedEntityKeyValues = NavTargetClass.GetSampleKeyArguments().ToArray();
            var relatedEntityPath = NavTargetClass.GetDefaultEntityPath(relatedEntityKeyValues);

            using (_mockedService = new MockService())
            {
                var baseAddress = _mockedService.GetBaseAddress().TrimEnd('/');
                var expectedJObject = new JObject();
                expectedJObject["@odata.id"] = baseAddress + relatedEntityPath;

                _mockedService
                    .SetupPostEntity(TargetEntity, entityKeyValues)
                    .SetupPostEntity(NavTargetEntity, relatedEntityKeyValues);

                var context = _mockedService.GetDefaultContext(Model);
                var fetcher = context.CreateFetcher(NavTargetFetcherType, propertyPath);
                var sourceInstance = context.CreateConcrete(ConcreteType);
                var targetInstance = context.CreateConcrete(NavTargetConcreteType);

                fetcher.InvokeMethod<Task>("UpdateLinkAsync", new object[] { sourceInstance, targetInstance, true }).Wait();

                _mockedService = _mockedService.OnPutUpdateLinkRequest(propertyPath, expectedJObject)
                    .RespondWithODataOk();

                context.SaveChangesAsync().Wait();
            }
        }
    }
}
