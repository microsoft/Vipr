using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Microsoft.MockService;
using Microsoft.MockService.Extensions.ODataV4;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Fetcher_Collection_Property : EntityTestBase
    {
        private MockService _mockedService;

        public Given_an_OdcmClass_Entity_Fetcher_Collection_Property()
        {
            base.Init();
        }

        [Fact]
        public void The_Collection_path_is_the_fetcher_path_plus_property_name()
        {
            var collectionProperty =
                Model.GetProperties()
                    .Where(p => p.Class.Kind == OdcmClassKind.Entity)
                    .Where(p => p.IsCollection)
                    .RandomElement();

            var entityPath = Any.UriPath(1);

            var propertyPath = "/" + entityPath + "/" + collectionProperty.Name;

            using (_mockedService = new MockService()
                    .SetupGetWithEmptyResponse(propertyPath)
                    .Start())
            {
                var fetcher = _mockedService
                    .GetDefaultContext(Model)
                    .CreateFetcher(Proxy.GetClass(collectionProperty.Class.Namespace, collectionProperty.Class.Name + "Fetcher"), entityPath);

                var propertyValue = fetcher.GetPropertyValue(collectionProperty.Name);

                propertyValue.InvokeMethod<Task>("ExecuteAsync").Wait();
            }
        }

        [Fact]
        public void Its_value_is_cached_and_reused_between_requests()
        {
            var collectionProperty =
                Model.GetProperties()
                    .Where(p => p.Class.Kind == OdcmClassKind.Entity)
                    .Where(p => p.IsCollection)
                    .RandomElement();

            var fetcher = DataServiceContextWrapperExtensions.CreateFetcher(null,
                Proxy.GetClass(collectionProperty.Class.Namespace, collectionProperty.Class.Name + "Fetcher"),
                collectionProperty.Name);

            fetcher.GetPropertyValue(collectionProperty.Name)
                .Should().Be(fetcher.GetPropertyValue(collectionProperty.Name), "Because the value should be cached.");
        }
    }
}
