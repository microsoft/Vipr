
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
    public class Given_an_OdcmClass_Entity_Collection_UpdateAsync_Method : EntityTestBase
    {
        private MockService _mockedService;
        private OdcmProperty _structuralInstanceProperty;

        public Given_an_OdcmClass_Entity_Collection_UpdateAsync_Method()
        {
            base.Init(m =>
            {
                _structuralInstanceProperty = Any.PrimitiveOdcmProperty(p =>
                {
                    p.Class = Class;
                    p.Projection = new OdcmProjection
                    {
                        Type = new OdcmPrimitiveType("String", OdcmNamespace.Edm)
                    };
                });
                Class.Properties.Add(_structuralInstanceProperty);
            });
        }

        [Fact]
        public void It_updates_an_entity_in_a_collection()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var expectedPath = Class.GetDefaultEntityPath(entityKeyValues);
            var newValue = Any.Word();

            var jobject = new JObject();
            jobject["@odata.type"] = "#" + Class.FullName;
            jobject[_structuralInstanceProperty.Name] = newValue;

            using (_mockedService = new MockService()
                    .SetupPostEntity(TargetEntity, entityKeyValues)
                    .OnPatchEntityRequest(expectedPath, jobject)
                        .RespondWithODataOk())
            {
                var context = _mockedService.GetDefaultContext(Model);
                var collection = context.CreateCollection(CollectionType, ConcreteType, Class.GetDefaultEntitySetPath());
                var instance = context.CreateConcrete(ConcreteType);

                instance.SetPropertyValue(_structuralInstanceProperty.Name, newValue);

                collection.InvokeMethod<Task>("UpdateAsync", new object[] { instance, System.Type.Missing }).Wait();
            }
        }

        [Fact]
        public void It_does_not_update_an_entity_when_delay_saving()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();

            using (_mockedService = new MockService()
                    .SetupPostEntity(TargetEntity, entityKeyValues))
            {
                var context = _mockedService.GetDefaultContext(Model);
                var collection = context.CreateCollection(CollectionType, ConcreteType, Class.GetDefaultEntitySetPath());
                var instance = context.CreateConcrete(ConcreteType);

                instance.SetPropertyValue(_structuralInstanceProperty.Name, Any.Word());

                //delay save when updating
                collection.InvokeMethod<Task>("UpdateAsync", new object[] { instance, true }).Wait();
            }
        }
    }
}
