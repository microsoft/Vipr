// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Its.Recipes;
using Microsoft.OData.ProxyExtensions;
using ODataV4TestService.SelfHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.OData.Client;
using Microsoft.OData.Core;
using ProxyExtensionsUnitTests.Extensions;
using Xunit;

namespace ProxyExtensionsUnitTests
{
    public class Given_Multiple_Queries_To_Execute_As_A_Batch : ProxyExtensionsUnitTestsBase
    {
        [Fact]
        public void When_Two_Successful_Queries_Are_Batched_Then_Two_BatchResults_Are_Successful()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products(8))
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                IReadOnlyQueryableSet<Product> productsSet = new ReadOnlyQueryableSet<Product>(
                                                                    context.CreateQuery<Product>("Products"),
                                                                    context);
                IReadOnlyQueryableSet<Product> prod3 = new ReadOnlyQueryableSet<Product>(
                                                                    context.CreateQuery<Product>("Products(3)"),
                                                                    context);

                var batchResult = context.ExecuteBatchAsync(productsSet, prod3).Result;
                batchResult.Count().Should().Be(2);

                foreach (var result in batchResult)
                {
                    result.SuccessResult.Should().NotBeNull();
                    result.FailureResult.Should().BeNull();
                }

                var products = batchResult[0].SuccessResult;
                products.CurrentPage.Count.Should().Be(8);

                products = batchResult[1].SuccessResult;
                products.CurrentPage.Count.Should().Be(1);
                (products.CurrentPage[0] as Product).Id.Should().Be(3);
            }
        }

        [Fact]
        public void When_Two_Unsuccessful_Queries_Are_Batched_Then_Two_BatchResults_Are_Failures()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products(8))
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                IReadOnlyQueryableSet<Product> productsSet = new ReadOnlyQueryableSet<Product>(
                                                                    context.CreateQuery<Product>("SomeRandomEntitySet"),
                                                                    context);
                IReadOnlyQueryableSet<Product> prod10 = new ReadOnlyQueryableSet<Product>(
                                                                    context.CreateQuery<Product>("Products(10)"),
                                                                    context);

                var batchResult = context.ExecuteBatchAsync(productsSet, prod10).Result;
                batchResult.Count().Should().Be(2);

                foreach (var result in batchResult)
                {
                    result.SuccessResult.Should().BeNull();
                    result.FailureResult.Should().NotBeNull();

                    result.FailureResult.
                    Should().BeOfType<DataServiceClientException>().
                    Which.
                    StatusCode.Should().Be(404);
                }
            }
        }

        [Fact]
        public void When_One_Unsuccessful_And_Successful_Queries_Are_Batched_Then_Two_Corresponding_BatchResults_Are_Failure_Success()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products(8))
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                IReadOnlyQueryableSet<Product> productsSet = new ReadOnlyQueryableSet<Product>(
                                                                    context.CreateQuery<Product>("SomeRandomEntitySet"),
                                                                    context);
                IReadOnlyQueryableSet<Product> prod3 = new ReadOnlyQueryableSet<Product>(
                                                                    context.CreateQuery<Product>("Products(3)"),
                                                                    context);

                var batchResult = context.ExecuteBatchAsync(productsSet, prod3).Result;
                batchResult.Count().Should().Be(2);

                batchResult[0].FailureResult.
                    Should().BeOfType<DataServiceClientException>().
                    Which.
                    StatusCode.Should().Be(404);

                batchResult[1].SuccessResult.
                    CurrentPage.Count.Should().Be(1);
                batchResult[1].SuccessResult.
                    CurrentPage[0].
                    Should().BeOfType<Product>().
                    Which.
                    Id.Should().Be(3);
            }
        }

        [Fact]
        public void When_Updates_To_Entities_Are_Batched_Then_Entities_Must_Be_Updated_On_Server()
        {
            var prods = Any.Products().ToArray();
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(prods.AsQueryable())
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                context.MergeOption = MergeOption.OverwriteChanges;
                var dQuery = context.CreateQuery<Product>("/" + "Products");
                var products = context.ExecuteAsync<Product, IProduct>(dQuery).Result;

                Product prod1 = products.CurrentPage[1] as Product;
                string newCategory = Any.AlphanumericString();
                prod1.Category = newCategory;
                prod1.CallOnPropertyChanged("Category");

                Product prod2 = products.CurrentPage[2] as Product;
                string newName = Any.CompanyName();
                prod2.Name = newName;
                prod2.CallOnPropertyChanged("Name");

                Product prod3 = products.CurrentPage[3] as Product;
                decimal newPrice = Any.Decimal();
                prod3.Price = newPrice;
                prod3.CallOnPropertyChanged("Price");

                var response = context.SaveChangesAsync(SaveChangesOptions.BatchWithSingleChangeset).Result;
                response.Count().Should().Be(3);
                response.IsBatchResponse.Should().BeTrue();

                var updatedProducts = context.ExecuteAsync<Product, IProduct>(dQuery).Result;
                updatedProducts.CurrentPage.Count.Should().Be(5);
                (updatedProducts.CurrentPage[1] as Product).Category.Should().Be(newCategory);
                (updatedProducts.CurrentPage[2] as Product).Name.Should().Be(newName);
                (updatedProducts.CurrentPage[3] as Product).Price.Should().Be(newPrice);
            }
        }
    }
}
