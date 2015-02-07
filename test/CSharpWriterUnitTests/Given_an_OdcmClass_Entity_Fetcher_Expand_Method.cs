using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Its.Recipes;
using Microsoft.OData.ProxyExtensions;
using ODataV4TestService.SelfHost;
using Vipr.Core;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Fetcher_Expand_Method : EntityTestBase
    {
        private IStartedScenario _mockedService;

        public Given_an_OdcmClass_Entity_Fetcher_Expand_Method()
        {
            Init();
        }

        [Fact]
        public void When_a_single_property_is_expanded_it_populates_the_DollarExpand_query_parameter()
        {
            var partialInstancePath = Any.UriPath(1);
            var instancePath = "/" + partialInstancePath;
            var keyValues = Class.GetSampleKeyArguments().ToArray();
            var navigationPropertyName = Class.NavigationProperties().Where(p => !p.IsCollection).RandomElement().Name;

            var param = Expression.Parameter(ConcreteInterface, "i");
            var navigationProperty = Expression.Property(param, navigationPropertyName);
            var lambda = Expression.Lambda(navigationProperty, new[] { param });

            using (_mockedService = new MockScenario()
                    .Setup(c => c.Request.Method == "GET" &&
                                c.Request.Path.Value == instancePath &&
                                c.Request.Query["$expand"] == navigationPropertyName,
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
                    .CreateFetcher(FetcherType, partialInstancePath);

                var result = fetcher.InvokeMethod<RestShallowObjectFetcher>("Expand", new []{lambda}, new []{ConcreteInterface}).InvokeMethod<Task>("ExecuteAsync").GetPropertyValue<EntityBase>("Result");

                result.ValidatePropertyValues(keyValues);
            }
        }

        [Fact]
        public void When_multiple_properties_are_expanded_it_populates_the_DollarExpand_query_parameter()
        {
            var partialInstancePath = Any.String(1);
            var instancePath = "/" + partialInstancePath;
            var keyValues = Class.GetSampleKeyArguments().ToArray();

            var navigationProperties =
                Class.NavigationProperties().Where(p => !p.IsCollection).RandomSubset(2).ToArray();

            var navigationPropertyName1 = navigationProperties[0].Name;
            var lambda1 = GetExpandLambda(navigationPropertyName1);

            var navigationPropertyName2 = navigationProperties[1].Name;
            var lambda2 = GetExpandLambda(navigationPropertyName2);

            using (_mockedService = new MockScenario()
                    .Setup(c => c.Request.Method == "GET" &&
                                c.Request.Path.Value == instancePath &&
                                c.Request.Query["$expand"].Split(',').SequenceEqual(new []{navigationPropertyName1, navigationPropertyName2}),
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
                    .CreateFetcher(FetcherType, partialInstancePath);

                var result = fetcher
                    .InvokeMethod<RestShallowObjectFetcher>("Expand", new[] { lambda1 }, new[] { ConcreteInterface })
                    .InvokeMethod<RestShallowObjectFetcher>("Expand", new[] { lambda2 }, new[] { ConcreteInterface })
                    .InvokeMethod<Task>("ExecuteAsync").GetPropertyValue<EntityBase>("Result");

                result.ValidatePropertyValues(keyValues);
            }
        }

        private LambdaExpression GetExpandLambda(string navigationPropertyName)
        {
            var param = Expression.Parameter(ConcreteInterface, "c");
            var navigationProperty = Expression.Property(param, navigationPropertyName);
            var lambda = Expression.Lambda(navigationProperty, new[] {param});
            return lambda;
        }
    }
}
