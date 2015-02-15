using System.Linq;
using CSharpWriter.Settings;
using Microsoft.Its.Recipes;
using Microsoft.MockService;
using Microsoft.OData.ProxyExtensions;
using Moq;
using Vipr.Core;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Navigation_Property_Key_forced_to_pascal_case : EntityTestBase
    {
        private MockService _mockedService;
        private string _camelCasedName;
        private readonly string _pascalCasedName;

        public Given_an_OdcmClass_Entity_Navigation_Property_Key_forced_to_pascal_case()
        {
            SetConfiguration(new CSharpWriterSettings
            {
                ForcePropertyPascalCasing = true
            });

            Init(m =>
            {
                var property = Class.Key.RandomElement();

                _camelCasedName = Any.Char('a', 'z') + property.Name;

                property.Rename(_camelCasedName);
            });

            _pascalCasedName = _camelCasedName.ToPascalCase();
        }

        [Fact(Skip = "https://github.com/Microsoft/Vipr/issues/26")]
        public void When_retrieved_through_Collection_GetById_method_then_request_is_sent_to_server_with_original_key_parameter_name()
        {
            var keyValues = Class.GetSampleKeyArguments().ToArray();

            using (_mockedService = new MockService()
                    .SetupGetEntity(TargetEntity, keyValues)
                    .Start())
            {
                var collection = _mockedService
                    .GetDefaultContext(Model)
                    .CreateCollection(CollectionType, ConcreteType, Class.GetDefaultEntitySetPath());

                var fetcher = collection.InvokeMethod<RestShallowObjectFetcher>("GetById",
                    keyValues.Select(k => k.Item2).ToArray());

                fetcher.ExecuteAsync().Wait();
            }
        }

        [Fact(Skip = "https://github.com/Microsoft/Vipr/issues/26")]
        public void When_retrieved_through_Collection_GetById_indexer_then_request_is_sent_to_server_with_original_key_parameter_name()
        {
            var keyValues = Class.GetSampleKeyArguments().ToArray();

            using (_mockedService = new MockService()
                    .SetupGetEntity(TargetEntity, keyValues)
                    .Start())
            {
                var collection = _mockedService
                    .GetDefaultContext(Model)
                    .CreateCollection(CollectionType, ConcreteType, Class.GetDefaultEntitySetPath());

                var fetcher =
                    collection.GetIndexerValue<RestShallowObjectFetcher>(keyValues.Select(k => k.Item2).ToArray());

                fetcher.ExecuteAsync().Wait();
            }
        }
    }
}