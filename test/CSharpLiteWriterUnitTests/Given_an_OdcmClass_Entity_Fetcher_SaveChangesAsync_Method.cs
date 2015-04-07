
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
    public class Given_an_OdcmClass_Entity_Fetcher_SaveChangesAsync_Method : EntityTestBase
    {
        private MockService _mockedService;
        private OdcmProperty _structuralInstanceProperty;

        public Given_an_OdcmClass_Entity_Fetcher_SaveChangesAsync_Method()
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
        public void It_updates_and_delay_saves_an_entity_from_its_own_path()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var expectedPath = Class.GetDefaultEntityPath(entityKeyValues);
            var newValue = Any.Word();

            var jobject = new JObject();
            jobject["@odata.type"] = "#" + Class.FullName;
            jobject[_structuralInstanceProperty.Name] = newValue;

            using (_mockedService = new MockService()
                    .SetupPostEntity(TargetEntity, entityKeyValues)
                    .OnInvokeMethodRequest("PATCH", expectedPath, null, jobject)
                        .RespondWithODataOk())
            {
                var context = _mockedService.GetDefaultContext(Model);
                var fetcher = context.CreateFetcher(FetcherType, Class.GetDefaultEntityPath(entityKeyValues));
                var instance = context.CreateConcrete(ConcreteType);

                instance.SetPropertyValue(_structuralInstanceProperty.Name, newValue);

                fetcher.InvokeMethod<Task>("UpdateAsync", new object[] { instance, true }).Wait();
                fetcher.InvokeMethod<Task>("SaveChangesAsync", new object[] { System.Type.Missing, System.Type.Missing }).Wait();
            }
        }
    }
}
