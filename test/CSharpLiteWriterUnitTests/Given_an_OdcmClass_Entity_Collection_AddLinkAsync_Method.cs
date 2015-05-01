
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
    public class Given_an_OdcmClass_Entity_Collection_AddLinkAsync_Method : EntityTestBase
    {
        private MockService _mockedService;
        private OdcmEntityClass _navPropertyClass;
        private OdcmProperty _navProperty;
        private System.Type _navPropertyFetcherType;
        private System.Type _navPropertyCollectionFetcherType;
        private System.Type _navPropertyConcreteType;
        private EntityArtifacts _navPropertyEntity;

        public Given_an_OdcmClass_Entity_Collection_AddLinkAsync_Method()
        {
            Init(odcmModel =>
            {
                // create a collection navigation property for 'Class' entity type.
                _navPropertyClass = Any.OdcmEntityClass(Namespace);
                odcmModel.AddType(_navPropertyClass);
                _navProperty = Any.OdcmProperty(p =>
                {
                    p.Class = Class;
                    p.Projection = new OdcmProjection()
                    {
                        Type = _navPropertyClass
                    };
                    p.IsCollection = true;
                });
                Class.Properties.Add(_navProperty);

                var serviceClass = Model.EntityContainer;

                var projection = new OdcmProjection() { Type = _navPropertyClass };

                serviceClass.Properties.Add(new OdcmProperty(_navPropertyClass.Name) { Class = serviceClass, Projection = projection });

                serviceClass.Properties.Add(new OdcmProperty(_navPropertyClass.Name + "s")
                {
                    Class = serviceClass,
                    Projection = projection,
                    IsCollection = true
                });
            });

            _navPropertyFetcherType = Proxy.GetClass(_navPropertyClass.Namespace, _navPropertyClass.Name + "Fetcher");
            _navPropertyCollectionFetcherType = Proxy.GetClass(_navPropertyClass.Namespace,
                _navPropertyClass.Name + "Collection");
            _navPropertyConcreteType = Proxy.GetClass(_navPropertyClass.Namespace, _navPropertyClass.Name);

            _navPropertyEntity = new EntityArtifacts()
            {
                Class = _navPropertyClass,
                ConcreteType = Proxy.GetClass(_navPropertyClass.Namespace, _navPropertyClass.Name),
                ConcreteInterface = Proxy.GetClass(_navPropertyClass.Namespace, "I" + _navPropertyClass.Name),
                FetcherType = Proxy.GetClass(_navPropertyClass.Namespace, _navPropertyClass.Name + "Fetcher"),
                FetcherInterface = Proxy.GetClass(_navPropertyClass.Namespace, "I" + _navPropertyClass.Name + "Fetcher"),
                CollectionType = Proxy.GetClass(_navPropertyClass.Namespace, _navPropertyClass.Name + "Collection"),
                CollectionInterface = Proxy.GetInterface(_navPropertyClass.Namespace, "I" + _navPropertyClass.Name + "Collection")
            };
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
            var propertyPath = Class.GetDefaultEntityPath(entityKeyValues) + "/" + _navProperty.Name;
            var entity2KeyValues = _navPropertyClass.GetSampleKeyArguments().ToArray();
            var entity2Path = _navPropertyClass.GetDefaultEntityPath(entity2KeyValues);

            using (_mockedService = new MockService())
            {
                var baseAddress = _mockedService.GetBaseAddress().TrimEnd('/');
                var expectedJObject = new JObject();
                expectedJObject["@odata.id"] = baseAddress + entity2Path;

                _mockedService
                    .SetupPostEntity(TargetEntity, entityKeyValues)
                    .SetupPostEntity(_navPropertyEntity, entity2KeyValues)
                    .OnPostAddLinkRequest(propertyPath, expectedJObject)
                    .RespondWithODataOk();

                var context = _mockedService.GetDefaultContext(Model);
                var collection = context.CreateCollection(_navPropertyCollectionFetcherType, _navPropertyConcreteType,
                    propertyPath);
                var sourceInstance = context.CreateConcrete(ConcreteType);
                var targetInstance = context.CreateConcrete(_navPropertyConcreteType);

                collection.InvokeMethod<Task>("AddLinkAsync", new object[] { sourceInstance, targetInstance, System.Type.Missing }).Wait();
            }
        }

        [Fact]
        public void It_does_not_add_a_link_when_delay_saving()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var propertyPath = Class.GetDefaultEntityPath(entityKeyValues) + "/" + _navProperty.Name;
            var entity2KeyValues = _navPropertyClass.GetSampleKeyArguments().ToArray();

            using (_mockedService = new MockService()
                    .SetupPostEntity(TargetEntity, entityKeyValues)
                    .SetupPostEntity(_navPropertyEntity, entity2KeyValues))
            {

                var context = _mockedService.GetDefaultContext(Model);
                var collection = context.CreateCollection(_navPropertyCollectionFetcherType, _navPropertyConcreteType,
                    propertyPath);
                var sourceInstance = context.CreateConcrete(ConcreteType);
                var targetInstance = context.CreateConcrete(_navPropertyConcreteType);
                
                collection.InvokeMethod<Task>("AddLinkAsync", new object[] { sourceInstance, targetInstance, true }).Wait();
            }
        }
    }
}
