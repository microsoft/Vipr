
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
    public class Given_an_OdcmClass_Entity_Fetcher_DeleteLinkAsync_Method : NavigationPropertyTestBase
    {
        private MockService _mockedService;

        public Given_an_OdcmClass_Entity_Fetcher_DeleteLinkAsync_Method()
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
            });

            NavTargetFetcherType = Proxy.GetClass(NavTargetClass.Namespace, NavTargetClass.Name + "Fetcher");
        }

        /*
         * DELETE request can be used to delete relationships/links between entities.
         * In this test 'DeleteLinkAsync' is called to delete a link between an entitytype and its 
         * single-valued navigation property.
         * Example request
         * DELETE http://services.odata.org/V4/TripPinServiceRW/People('russellwhyte')/Photo/$ref
         * This request deletes the link between entity "People('russellwhyte')" and its single-valued navigation 
         * property called 'Photo'.
         * 
         * Spec - http://docs.oasis-open.org/odata/odata/v4.0/errata02/os/complete/part1-protocol/odata-v4.0-errata02-os-part1-protocol-complete.html#_Toc406398334
         */
        [Fact]
        public void It_deletes_the_link_between_entity_and_singlevalued_navigation_property()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var propertyPath = Class.GetDefaultEntityPath(entityKeyValues) + "/" + NavigationProperty.Name;

            using (_mockedService = new MockService()
                    .SetupPostEntity(TargetEntity, entityKeyValues)
                    .OnDeleteLinkRequest(propertyPath)
                        .RespondWithODataOk())
            {
                var context = _mockedService.GetDefaultContext(Model);
                var fetcher = context.CreateFetcher(NavTargetFetcherType, propertyPath);
                var sourceInstance = context.CreateConcrete(ConcreteType);

                fetcher.InvokeMethod<Task>("DeleteLinkAsync", new object[] { sourceInstance, System.Type.Missing }).Wait();
            }
        }

        [Fact]
        public void It_does_not_delete_a_link_when_delay_saving()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var propertyPath = Class.GetDefaultEntityPath(entityKeyValues) + "/" + NavigationProperty.Name;

            using (_mockedService = new MockService()
                    .SetupPostEntity(TargetEntity, entityKeyValues))
            {
                var context = _mockedService.GetDefaultContext(Model);
                var fetcher = context.CreateFetcher(NavTargetFetcherType, propertyPath);
                var sourceInstance = context.CreateConcrete(ConcreteType);

                //delay save when deleting the link
                fetcher.InvokeMethod<Task>("DeleteLinkAsync", new object[] { sourceInstance, true }).Wait();
            }
        }

        [Fact]
        public void It_deletes_a_link_when_delay_saving_and_calling_SaveChangesAsync()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var propertyPath = Class.GetDefaultEntityPath(entityKeyValues) + "/" + NavigationProperty.Name;

            using (_mockedService = new MockService()
                    .SetupPostEntity(TargetEntity, entityKeyValues))
            {
                var context = _mockedService.GetDefaultContext(Model);
                var fetcher = context.CreateFetcher(NavTargetFetcherType, propertyPath);
                var sourceInstance = context.CreateConcrete(ConcreteType);

                fetcher.InvokeMethod<Task>("DeleteLinkAsync", new object[] { sourceInstance, true }).Wait();

                _mockedService = _mockedService.OnDeleteLinkRequest(propertyPath)
                    .RespondWithODataOk();

                context.SaveChangesAsync().Wait();
            }
        }
    }
}
