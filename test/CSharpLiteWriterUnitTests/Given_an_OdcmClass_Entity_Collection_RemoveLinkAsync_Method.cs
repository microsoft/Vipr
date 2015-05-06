
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
    public class Given_an_OdcmClass_Entity_Collection_RemoveLinkAsync_Method : NavigationPropertyTestBase
    {
        private MockService _mockedService;

        public Given_an_OdcmClass_Entity_Collection_RemoveLinkAsync_Method()
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
         * DELETE request can be used to remove a reference from a collection-valued navigation property.
         * In this test 'RemoveLinkAsync' is called to delete an entity reference from a collection-valued navigation property.
         * Example request
         * DELETE http://services.odata.org/V4/TripPinServiceRW/People('russellwhyte')/Friends/$ref?$id=http://services.odata.org/V4/TripPinServiceRW/People('scottketchum')
         * This request removes the link "People('scottketchum')" from the collection-valued navigation property called 'Friends'.
         * 
         * Spec - http://docs.oasis-open.org/odata/odata/v4.0/errata02/os/complete/part1-protocol/odata-v4.0-errata02-os-part1-protocol-complete.html#_Toc406398334
         */

        [Fact]
        public void It_deletes_a_link_from_a_collection_valued_navigation_property()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var propertyPath = Class.GetDefaultEntityPath(entityKeyValues) + "/" + NavigationProperty.Name;
            var relatedEntityKeyValues = NavTargetClass.GetSampleKeyArguments().ToArray();
            var relatedEntityPath = NavTargetClass.GetDefaultEntityPath(relatedEntityKeyValues);

            using (_mockedService = new MockService())
            {
                var baseAddress = _mockedService.GetBaseAddress().TrimEnd('/');
                var idQueryPath = baseAddress + relatedEntityPath;
                

                _mockedService
                    .SetupPostEntity(TargetEntity, entityKeyValues)
                    .SetupPostEntity(NavTargetEntity, relatedEntityKeyValues)
                    .OnDeleteLinkRequest(propertyPath, idQueryPath)
                    .RespondWithODataOk();

                var context = _mockedService.GetDefaultContext(Model);
                var collection = context.CreateCollection(NavTargetCollectionType, NavTargetConcreteType,
                    propertyPath);
                var sourceInstance = context.CreateConcrete(ConcreteType);
                var targetInstance = context.CreateConcrete(NavTargetConcreteType);
                
                collection.InvokeMethod<Task>("RemoveLinkAsync", new object[] { sourceInstance, targetInstance, System.Type.Missing }).Wait();
            }
        }

        [Fact]
        public void It_does_not_remove_a_link_when_delay_saving()
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
                
                collection.InvokeMethod<Task>("RemoveLinkAsync", new object[] { sourceInstance, targetInstance, true }).Wait();
            }
        }

        [Fact]
        public void It_removes_a_link_when_delay_saving_and_calling_SaveChangesAsync()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var propertyPath = Class.GetDefaultEntityPath(entityKeyValues) + "/" + NavigationProperty.Name;
            var relatedEntityKeyValues = NavTargetClass.GetSampleKeyArguments().ToArray();
            var relatedEntityPath = NavTargetClass.GetDefaultEntityPath(relatedEntityKeyValues);

            using (_mockedService = new MockService())
            {
                var baseAddress = _mockedService.GetBaseAddress().TrimEnd('/');
                var idQueryPath = baseAddress + relatedEntityPath;

                _mockedService
                    .SetupPostEntity(TargetEntity, entityKeyValues)
                    .SetupPostEntity(NavTargetEntity, relatedEntityKeyValues);

                var context = _mockedService.GetDefaultContext(Model);
                var collection = context.CreateCollection(NavTargetCollectionType, NavTargetConcreteType,
                    propertyPath);
                var sourceInstance = context.CreateConcrete(ConcreteType);
                var targetInstance = context.CreateConcrete(NavTargetConcreteType);

                collection.InvokeMethod<Task>("RemoveLinkAsync", new object[] { sourceInstance, targetInstance, true }).Wait();

                _mockedService = _mockedService.OnDeleteLinkRequest(propertyPath, idQueryPath)
                    .RespondWithODataOk();

                context.SaveChangesAsync().Wait();
            }
        }
    }
}
