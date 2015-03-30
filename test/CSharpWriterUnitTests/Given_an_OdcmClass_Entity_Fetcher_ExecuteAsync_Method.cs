using System.Linq;
using System.Threading.Tasks;
using Microsoft.MockService;
using Microsoft.OData.ProxyExtensions;
using Xunit;

namespace CSharpWriterUnitTests
{
    /// <summary>
    /// Summary description for Given_an_OdcmModel
    /// </summary>
    public class Given_an_OdcmClass_Entity_Fetcher_ExecuteAsync_Method : EntityTestBase
    {
        private MockService _mockedService;

        public Given_an_OdcmClass_Entity_Fetcher_ExecuteAsync_Method()
        {
            Init();
        }

        [Fact]
        public void It_retrieves_a_value_from_its_own_path()
        {
            var keyValues = Class.GetSampleKeyArguments().ToArray();

            using (_mockedService = new MockService()
                    .SetupGetEntity(TargetEntity, keyValues))
            {
                var fetcher = _mockedService
                    .GetDefaultContext(Model)
                    .CreateFetcher(FetcherType, Class.GetDefaultEntityPath(keyValues));

                var result = fetcher.InvokeMethod<Task>("ExecuteAsync").GetPropertyValue<EntityBase>("Result");

                result.ValidatePropertyValues(keyValues);
            }
        }
    }
}
