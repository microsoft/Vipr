using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Microsoft.OData.Client;
using Microsoft.OData.ProxyExtensions;
using Xunit;

namespace ProxyExtensionsUnitTests
{
    public class Given_a_RestShallowObjectFetcher_Initialized
    {
        private TestRestShallowObjectFetcher _fetcher;
        private string _path;
        private DataServiceContextWrapper _context;
        private Uri _baseUri;

        public Given_a_RestShallowObjectFetcher_Initialized()
        {
            _path = Any.String(1);

            _baseUri = Any.Uri(allowQuerystring: false);

            _context = new DataServiceContextWrapper(_baseUri, Any.EnumValue<ODataProtocolVersion>(),
                () => Task.FromResult(Any.String()));

            _fetcher = new TestRestShallowObjectFetcher();

            _fetcher.Initialize(_context, _path);
            
        }

        [Fact]
        public void CreateQuery_returns_a_query_with_the_Fetchers_Context_and_Uri()
        {
            var query = _fetcher.CreateQuery<Product, IProduct>();

            query.Context
                .Should().Be(_context);

            query.Query.RequestUri
                .Should().Be(_fetcher.GetUrl());
        }

        [Fact]
        public void When_Property_is_null_GetPath_returns_initialized_path()
        {
            _fetcher.GetPath(null)
                .Should().Be(_path);
        }

        [Fact]
        public void When_Property_is_not_null_GetPath_returns_initialized_path_slash_Property()
        {
            var property = Any.String();

            _fetcher.GetPath(property)
                .Should().Be(_path + "/" + property);
        }

        [Fact]
        public void When_the_path_does_not_have_a_trailing_slash_GetUrl_returns_the_path_appended_to_the_Context_base_uri()
        {
            var baseUriString = Any.Uri(allowQuerystring: false).AbsoluteUri.TrimEnd('/');

            var context = new DataServiceContextWrapper(new Uri(baseUriString), Any.EnumValue<ODataProtocolVersion>(),
                () => Task.FromResult(Any.String()));

            var fetcher = new TestRestShallowObjectFetcher();

            fetcher.Initialize(context, _path);
            fetcher.GetUrl().AbsoluteUri
                .Should().Be(new Uri(baseUriString + "/" + _path).AbsoluteUri);
        }

        [Fact]
        public void When_the_path_has_a_trailing_slash_GetUrl_returns_the_path_appended_to_the_Context_base_uri_with_a_single_slash()
        {
            var baseUriString = Any.Uri(allowQuerystring: false).AbsoluteUri.TrimEnd('/');

            var context = new DataServiceContextWrapper(new Uri(baseUriString + "/"), Any.EnumValue<ODataProtocolVersion>(),
                () => Task.FromResult(Any.String()));

            var fetcher = new TestRestShallowObjectFetcher();

            fetcher.Initialize(context, _path);
            fetcher.GetUrl().AbsoluteUri
                .Should().Be(new Uri(baseUriString + "/" + _path).AbsoluteUri);
        }
    }
}