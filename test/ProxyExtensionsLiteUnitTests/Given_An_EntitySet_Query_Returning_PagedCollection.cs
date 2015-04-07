// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Its.Recipes;
using Microsoft.OData.ProxyExtensions.Lite;
using ODataV4TestService.SelfHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace ProxyExtensionsUnitTests
{
    public class Given_An_EntitySet_Query_Returning_PagedCollection : ProxyExtensionsUnitTestsBase
    {
        [Fact]
        public void When_PageSize_LesserThan_EntitySet_Count_Then_Return_PagedCollection_With_Multiple_Pages()
        {
            int pageSize = 10;
            int productsCount = Any.Int(pageSize + 1, 50);
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products(productsCount))
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                var dQuery = context.CreateQuery<Product>("/" + "Products");
                var products = context.ExecuteAsync<Product, IProduct>(dQuery).Result;
                int totalPagesCount = 0;

                while (products.MorePagesAvailable)
                {
                    totalPagesCount++;
                    products.CurrentPage.Count.Should().Be(pageSize);
                    products = products.GetNextPageAsync().Result;
                }

                // 'productsCount' is divisible by 'pageSize'.
                // Then the last page has 'pageSize' elements in it.
                if (productsCount % pageSize == 0)
                {
                    totalPagesCount.Should().Be((productsCount / pageSize) - 1);
                    products.CurrentPage.Count.Should().Be(pageSize);
                }
                else
                {
                    totalPagesCount.Should().Be(productsCount / pageSize);
                    products.CurrentPage.Count.Should().Be(productsCount % pageSize);
                }

                //When no more pages are available 'GetNextPageAsync' should return null
                products.GetNextPageAsync().Result.Should().BeNull();
            }
        }

        [Fact]
        public void When_PageSize_GreaterThan_EntitySet_Count_Then_Return_PagedCollection_With_Single_Page()
        {
            int pageSize = 10;
            int productsCount = Any.Int(1, pageSize - 1);
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products(productsCount))
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                var dQuery = context.CreateQuery<Product>("/" + "Products");
                var products = context.ExecuteAsync<Product, IProduct>(dQuery).Result;

                products.MorePagesAvailable.Should().BeFalse();
                products.CurrentPage.Count.Should().Be(productsCount);

                //When no more pages are available 'GetNextPageAsync' should return null
                products.GetNextPageAsync().Result.Should().BeNull();
            }
        }

        [Fact]
        public void When_PageSize_EqualsTo_EntitySet_Count_Then_Return_PagedCollection_With_Single_Page()
        {
            int productsCount = 10;
            int pageSize = 10;
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products(productsCount))
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                var dQuery = context.CreateQuery<Product>("/" + "Products");
                IPagedCollection products = (IPagedCollection)context.ExecuteAsync<Product, IProduct>(dQuery).Result;

                products.MorePagesAvailable.Should().BeFalse();
                products.CurrentPage.Count.Should().Be(pageSize);

                //When no more pages are available 'GetNextPageAsync' should return null
                products.GetNextPageAsync().Result.Should().BeNull();
            }
        }
    }
}
