
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
    public class Given_an_OdcmClass_Entity_Collection_AddLinkAsync_Method : NavigationPropertyTestBase
    {
        private MockService _mockedService;

        public Given_an_OdcmClass_Entity_Collection_AddLinkAsync_Method()
        {
            Init(odcmModel =>
            {
                // create a collection navigation property for 'Class' entity type.
                NavTargetClass = Any.OdcmEntityClass(Namespace);
                odcmModel.AddType(NavTargetClass);
                NavigationProperty = Any.OdcmProperty(p =>
                {
                    p.Class = Class;
                    p.Projection = new OdcmProjection()
                    {
                        Type = NavTargetClass
                    };
                    p.IsCollection = true;
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
         * POST request can be used to a add a reference to a collection-valued navigation property
         * In this test 'AddLinkAsync' is called to add a link to a collection-valued navigation property.
         * Example request
         * POST http://services.odata.org/V4/TripPinServiceRW/People('russellwhyte')/Friends/$ref
         * Request Body
         * {"@odata.id":"http://services.odata.org/V4/TripPinServiceRW/People('scottketchum')"}
         * 
         * This request add the link "People('scottketchum')" to the collection-valued navigation 
         * property called 'Friends'.
         * 
         * Spec - http://docs.oasis-open.org/odata/odata/v4.0/errata02/os/complete/part1-protocol/odata-v4.0-errata02-os-part1-protocol-complete.html#_Toc406398333
         */

        [Fact]
        public void It_adds_a_link_to_a_collection_valued_navigation_property()
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
                    .OnPostAddLinkRequest(propertyPath, expectedJObject)
                    .RespondWithODataOk();

                var context = _mockedService.GetDefaultContext(Model);
                var collection = context.CreateCollection(NavTargetCollectionType, NavTargetConcreteType,
                    propertyPath);
                var sourceInstance = context.CreateConcrete(ConcreteType);
                var targetInstance = context.CreateConcrete(NavTargetConcreteType);

                collection.InvokeMethod<Task>("AddLinkAsync", new object[] { sourceInstance, targetInstance, System.Type.Missing }).Wait();
            }
        }

        [Fact]
        public void It_does_not_add_a_link_when_delay_saving()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var propertyPath = Class.GetDefaultEntityPath(entityKeyValues) + "/" + NavigationProperty.Name;
            var relatedEntityKeyValues = NavTargetClass.GetSampleKeyArguments().ToArray();

            using (_mockedService = new MockService()
                    .SetupPostEntity(TargetEntity, entityKeyValues)
                    .SetupPostEntity(NavTargetEntity, relatedEntityKeyValues))
            {

                var context = _mockedService.GetDefaultContext(Model);
                var collection = context.CreateCollection(NavTargetCollectionType, NavTargetConcreteType,
                    propertyPath);
                var sourceInstance = context.CreateConcrete(ConcreteType);
                var targetInstance = context.CreateConcrete(NavTargetConcreteType);
                
                collection.InvokeMethod<Task>("AddLinkAsync", new object[] { sourceInstance, targetInstance, true }).Wait();
            }
        }
    }
}
