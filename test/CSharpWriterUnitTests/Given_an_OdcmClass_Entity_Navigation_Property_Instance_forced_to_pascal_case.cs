using System;
using System.Linq;
using Microsoft.Its.Recipes;
using Microsoft.OData.ProxyExtensions;
using Moq;
using ODataV4TestService.SelfHost;
using Vipr.Core;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Navigation_Property_Instance_forced_to_pascal_case : EntityTestBase
    {
        private IStartedScenario _mockedService;
        private string _camelCasedName;
        private readonly string _pascalCasedName;

        public Given_an_OdcmClass_Entity_Navigation_Property_Instance_forced_to_pascal_case()
        {
            var configurationProviderMock = new Mock<IConfigurationProvider>();
                configurationProviderMock.Setup(c => c.ForcePropertyPascalCasing).Returns(true);
                ConfigurationProvider = configurationProviderMock.Object;

                Init(m =>
                {
                    var property = Class.NavigationProperties().Where(p => !p.IsCollection).RandomElement();

                    Class.Properties.Remove(property);

                    Class.Properties.Add(
                        new OdcmProperty(_camelCasedName = Any.Char('a', 'z') + property.Name)
                        {
                            Class = property.Class,
                            ReadOnly = property.ReadOnly,
                            Type = property.Type
                        });
                });

            _pascalCasedName = _camelCasedName.ToPascalCase();
        }
        
        [Fact]
        public void When_retrieved_through_Fetcher_then_request_is_sent_to_server_with_original_name()
        {
            var entityPath = Any.UriPath(1);
            var expectedPath = "/" + entityPath + "/" + _camelCasedName;
            var keyValues = Class.GetSampleKeyArguments().ToArray();

            using (_mockedService = new MockScenario()
                    .Setup(c => c.Request.Method == "GET" &&
                                c.Request.Path.Value == expectedPath,
                           (b, c) =>
                           {
                               c.Response.StatusCode = 200;
                               c.Response.WithDefaultODataHeaders();
                               c.Response.Write(ConcreteType.AsJson(b, keyValues));
                           })
                    .Start())
            {
                var fetcher = _mockedService
                    .GetContext()
                    .UseJson(Model.ToEdmx(), true)
                    .CreateFetcher(FetcherType, entityPath);

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

            using (_mockedService = new MockScenario()
                .Setup(c => c.Request.Method == "POST" &&
                            c.Request.Path.Value == entitySetPath,
                    (b, c) =>
                    {
                        c.Response.StatusCode = 201;
                        c.Response.WithDefaultODataHeaders();
                        c.Response.Write(ConcreteType.AsJson(b, entityKeyValues));
                    })
                .Setup(c => c.Request.Method == "GET" &&
                            c.Request.Path.Value == expectedPath,
                    (b, c) =>
                    {
                        c.Response.StatusCode = 200;
                        c.Response.WithDefaultODataHeaders();
                        c.Response.Write(ConcreteType.AsJson(b, keyValues));
                    })
                .Start())
            {
                var instance = _mockedService
                    .GetContext()
                    .UseJson(Model.ToEdmx(), true)
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
            var entitySetName = Class.Name + "s";
            var entitySetPath = "/" + entitySetName;
            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var entityPath = string.Format("{0}({1})", entitySetPath, ODataKeyPredicate.AsString(entityKeyValues));
            var expectedPath = entityPath + "/" + _camelCasedName;
            var keyValues = Class.GetSampleKeyArguments().ToArray();

            using (_mockedService = new MockScenario()
                    .Setup(c => c.Request.Method == "POST" &&
                                c.Request.Path.Value == entitySetPath,
                           (b, c) =>
                           {
                               c.Response.StatusCode = 201;
                               c.Response.WithDefaultODataHeaders();
                               c.Response.Write(ConcreteType.AsJson(b, entityKeyValues));
                           })
                    .Setup(c => c.Request.Method == "GET" &&
                                c.Request.Path.Value == expectedPath,
                           (b, c) =>
                           {
                               c.Response.StatusCode = 200;
                               c.Response.WithDefaultODataHeaders();
                               c.Response.Write(ConcreteType.AsJson(b, keyValues));
                           })
                    .Start())
            {
                var instance = _mockedService
                    .GetContext()
                    .UseJson(Model.ToEdmx(), true)
                    .CreateConcrete(ConcreteType);

                instance.SetPropertyValues(Class.GetSampleKeyArguments());

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

            using (_mockedService = new MockScenario()
                    .Setup(c => c.Request.Method == "POST" &&
                                c.Request.Path.Value == entitySetPath,
                           (b, c) =>
                           {
                               c.Response.StatusCode = 201;
                               c.Response.WithDefaultODataHeaders();
                               c.Response.Write(ConcreteType.AsJson(b, entityKeyValues));
                           })
                    .Setup(c => c.Request.Method == "PATCH" &&
                                c.Request.Path.Value == expectedPath,
                           (b, c) =>
                           {
                               c.Response.StatusCode = 200;
                               c.Response.WithDefaultODataHeaders();
                           })
                    .Start())
            {
                var context = _mockedService
                    .GetContext()
                    .UseJson(Model.ToEdmx(), true);
                var instance = context
                    .CreateConcrete(ConcreteType);

                var relatedInstance = Activator.CreateInstance(ConcreteType);

                instance.SetPropertyValue(_pascalCasedName, relatedInstance);

                instance.UpdateAsync().Wait();
            }
        }
    }
}