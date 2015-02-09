using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Its.Recipes;
using Microsoft.MockService;
using Microsoft.MockService.Extensions.ODataV4;
using Microsoft.OData.ProxyExtensions;
using Moq;
using Vipr.Core;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Navigation_Property_Instance_forced_to_pascal_case : EntityTestBase
    {
        private MockService _mockedService;
        private OdcmProperty _camelCasedProperty;
        private string _camelCasedName;
        private readonly string _pascalCasedName;

        public Given_an_OdcmClass_Entity_Navigation_Property_Instance_forced_to_pascal_case()
        {
            var configurationProviderMock = new Mock<IConfigurationProvider>();
                configurationProviderMock.Setup(c => c.ForcePropertyPascalCasing).Returns(true);
                ConfigurationProvider = configurationProviderMock.Object;

                Init(m =>
                {
                    _camelCasedProperty = Class.NavigationProperties().Where(p => !p.IsCollection).RandomElement();

                    _camelCasedName = Any.Char('a', 'z') + _camelCasedProperty.Name;

                    _camelCasedProperty = _camelCasedProperty.Rename(_camelCasedName);
                });

            _pascalCasedName = _camelCasedName.ToPascalCase();
        }

        [Fact]
        public void When_retrieved_through_Fetcher_then_request_is_sent_to_server_with_original_name()
        {
            var keyValues = Class.GetSampleKeyArguments().ToArray();

            using (_mockedService = new MockService()
                    .SetupGetEntityProperty(TargetEntity, keyValues, _camelCasedProperty)
                    .Start())
            {
                var fetcher = _mockedService
                    .GetDefaultContext(Model)
                    .CreateFetcher(FetcherType, TargetEntity.Class.GetDefaultEntityPath(keyValues));

                var propertyFetcher = fetcher.GetPropertyValue<RestShallowObjectFetcher>(_pascalCasedName);

                propertyFetcher.ExecuteAsync().Wait();
            }
        }

        [Fact(Skip = "Issue #24 https://github.com/Microsoft/vipr/issues/24")]
        public void When_retrieved_through_Concrete_ConcreteInterface_Property_then_request_is_sent_with_original_name()
        {
            var entitySetName = Class.Name + "s";
            var entitySetPath = "/" + entitySetName;
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var entityPath = string.Format("{0}({1})", entitySetPath, ODataKeyPredicate.AsString(entityKeyValues));
            var expectedPath = entityPath + "/" + _camelCasedName;
            var keyValues = Class.GetSampleKeyArguments().ToArray();

            using (_mockedService = new MockService()
                .SetupPostEntity(TargetEntity, entityKeyValues)
                .SetupGetEntity(TargetEntity, keyValues)
                .Start())
            {
                var instance = _mockedService
                    .GetDefaultContext(Model)
                    .CreateConcrete(ConcreteType);

                instance.SetPropertyValues(Class.GetSampleKeyArguments());

                var propertyValue = instance.GetPropertyValue<RestShallowObjectFetcher>(ConcreteInterface,
                    _pascalCasedName);

                propertyValue.ExecuteAsync().Wait();
            }
        }

        [Fact]
        public void When_retrieved_through_Concrete_then_request_is_sent_to_server_with_original_name()
        {
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();

            using (_mockedService = new MockService()
                    .SetupPostEntity(TargetEntity, entityKeyValues)
                    .SetupGetEntityProperty(TargetEntity, entityKeyValues, _camelCasedProperty)
                    .Start())
            {
                var instance = _mockedService
                    .GetDefaultContext(Model)
                    .CreateConcrete(ConcreteType);

                instance.SetPropertyValues(entityKeyValues);

                var propertyFetcher = instance.GetPropertyValue<RestShallowObjectFetcher>(FetcherInterface,
                    _pascalCasedName);

                propertyFetcher.ExecuteAsync().Wait();
            }
        }

        [Fact]
        public void When_updated_through_Concrete_accessor_then_request_is_sent_to_server_with_original_name()
        {
            var entitySetName = Class.Name + "s";
            var entitySetPath = "/" + entitySetName;
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var entityPath = string.Format("{0}({1})", entitySetPath, ODataKeyPredicate.AsString(entityKeyValues));
            var expectedPath = entityPath;

            using (_mockedService = new MockService()
                    .SetupPostEntity(TargetEntity, entityKeyValues)
                    .SetupPatchEntityChanges(expectedPath)
                    .Start())
            {
                var context = _mockedService
                    .GetDefaultContext(Model);

                var instance = context
                    .CreateConcrete(ConcreteType);

                var relatedInstance = Activator.CreateInstance(ConcreteType);

                instance.SetPropertyValue(_pascalCasedName, relatedInstance);

                instance.UpdateAsync().Wait();
            }
        }

        [Fact(Skip = "https://github.com/Microsoft/Vipr/issues/27")]
        public void When_a_renamed_property_is_expanded_it_populates_the_DollarExpand_query_parameter()
        {
            var keyValues = Class.GetSampleKeyArguments().ToArray();

            var param = Expression.Parameter(ConcreteInterface, "i");
            var navigationProperty = Expression.Property(param, _pascalCasedName);
            var lambda = Expression.Lambda(navigationProperty, new[] { param });

            using (_mockedService = new MockService()
                    .SetupGetEntity(TargetEntity, keyValues, new[]{_camelCasedName})
                    .Start())
            {
                var fetcher = _mockedService
                    .GetDefaultContext(Model)
                    .CreateFetcher(FetcherType, Class.GetDefaultEntityPath(keyValues));

                fetcher.InvokeMethod<RestShallowObjectFetcher>("Expand", new[] {lambda}, new[] {ConcreteInterface})
                    .InvokeMethod<Task>("ExecuteAsync").Wait();
            }
        }
    }
}