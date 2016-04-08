
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
    public class Given_an_OdcmClass_Entity_Fetcher_DeleteAsync_Method : EntityTestBase
    {
        private MockService _mockedService;
//        private OdcmProperty _structuralInstanceProperty;

        public Given_an_OdcmClass_Entity_Fetcher_DeleteAsync_Method()
        {
            Init();
        }

        [Fact]
        public void It_deletes_an_entity_from_its_own_path()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var expectedPath = Class.GetDefaultEntityPath(entityKeyValues);
            

            using (_mockedService = new MockService()
                    .SetupPostEntity(TargetEntity, entityKeyValues)
                    .OnDeleteEntityRequest(expectedPath)
                        .RespondWithODataOk())
            {
                var context = _mockedService.GetDefaultContext(Model);
                var fetcher = context.CreateFetcher(FetcherType, Class.GetDefaultEntityPath(entityKeyValues));
                var instance = context.CreateConcrete(ConcreteType);

                fetcher.InvokeMethod<Task>("DeleteAsync", new object[] { instance, System.Type.Missing }).Wait();
            }
        }

        [Fact]
        public void It_does_not_delete_an_entity_when_delay_saving()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();

            using (_mockedService = new MockService()
                    .SetupPostEntity(TargetEntity, entityKeyValues))
            {
                var context = _mockedService.GetDefaultContext(Model);
                var fetcher = context.CreateFetcher(FetcherType, Class.GetDefaultEntityPath(entityKeyValues));
                var instance = context.CreateConcrete(ConcreteType);

                //delay save when deleting
                fetcher.InvokeMethod<Task>("DeleteAsync", new object[] { instance, true }).Wait();
            }
        }

        [Fact]
        public void It_deletes_an_entity_when_delay_saving_and_calling_SaveChangesAsync()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var expectedPath = Class.GetDefaultEntityPath(entityKeyValues);


            using (_mockedService = new MockService()
                    .SetupPostEntity(TargetEntity, entityKeyValues))
            {
                var context = _mockedService.GetDefaultContext(Model);
                var fetcher = context.CreateFetcher(FetcherType, Class.GetDefaultEntityPath(entityKeyValues));
                var instance = context.CreateConcrete(ConcreteType);

                fetcher.InvokeMethod<Task>("DeleteAsync", new object[] { instance, true }).Wait();

                _mockedService = _mockedService.OnDeleteEntityRequest(expectedPath)
                    .RespondWithODataOk();

                context.SaveChangesAsync().Wait();
            }
        }
    }
}
