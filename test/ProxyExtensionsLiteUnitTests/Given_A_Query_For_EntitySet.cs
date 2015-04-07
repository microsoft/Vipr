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
using Microsoft.Spatial;
using Xunit;

namespace ProxyExtensionsUnitTests
{
    public class Given_A_Query_For_EntitySet : ProxyExtensionsUnitTestsBase
    {
        [Fact]
        public void When_Querying_For_Existing_EntitySet_Then_Return_The_EntitySet()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products())
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                var dQuery = context.CreateQuery<Product>("/" + "Products");

                //query for all the products
                var products = context.ExecuteAsync<Product, IProduct>(dQuery).Result;
                products.CurrentPage.Count.Should().Be(5);
                (products.CurrentPage[2] as Product).Id.Should().Be(3);
            }
        }

        [Fact]
        public void When_Querying_For_Existing_EntitySet_Using_EntityType_Interface_Then_Return_The_EntitySet()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products())
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                var dQuery = context.CreateQuery<Product>("/" + "Products");
                IReadOnlyQueryableSet<IProduct> readOnlySet = new ReadOnlyQueryableSet<IProduct>(dQuery, context);

                var products = readOnlySet.ExecuteAsync().Result;
                products.CurrentPage.Count.Should().Be(5);
                (products.CurrentPage[2] as Product).Id.Should().Be(3);
            }
        }

        [Fact]
        public void When_Querying_For_NonExistent_EntitySet_Then_Throw_DataServiceQueryException_With_NotFound_Status_Code()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products())
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());

                var action = new Action(() =>
                {
                    var dQuery = context.CreateQuery<Product>("/" + "RandomEntitySet");
                    var products = context.ExecuteAsync<Product, IProduct>(dQuery).Result;
                });

                //check for AggregateException with DataServiceQueryException as InnerException.
                //Also retrieve the DataServiceQueryException InnerException.
                DataServiceQueryException innerException = action.ShouldThrow<AggregateException>()
                    .WithInnerException<DataServiceQueryException>()
                    .And.InnerException as DataServiceQueryException;

                //Check for 'Not Found' code.
                innerException.Response.StatusCode.Should().Be(404);
            }
        }
    }
}
