using System;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Xunit;

namespace ProxyExtensionsUnitTests
{
    public class Given_a_RestShallowObjectFetcher_Uninitialized
    {
        private TestRestShallowObjectFetcher _fetcher;

        public Given_a_RestShallowObjectFetcher_Uninitialized()
        {
            _fetcher = new TestRestShallowObjectFetcher();
        }

        [Fact]
        public void CreateQuery_throws_InvalidOperationException()
        {
            Action callCreateQuery = () => _fetcher.CreateQuery<Product, IProduct>();

            callCreateQuery.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void GetPath_throws_InvalidOperationException()
        {
            Action callCreateQuery = () => _fetcher.GetPath(Any.String());

            callCreateQuery.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void GetUrl_throws_InvalidOperationException()
        {
            Action callCreateQuery = () => _fetcher.GetUrl();

            callCreateQuery.ShouldThrow<InvalidOperationException>();
        }
    }
}
