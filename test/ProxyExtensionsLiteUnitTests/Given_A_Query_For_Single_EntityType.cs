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
    public class Given_A_Query_For_Single_EntityType : ProxyExtensionsUnitTestsBase
    {
        [Fact]
        public void When_Querying_For_Existing_EntityType_Then_Return_The_EntityType()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products())
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());

                //query for a single product based on the 'Id'
                var dQuery = context.CreateQuery<Product>("/" + "Products(3)");
                Product prod3 = context.ExecuteSingleAsync<Product, IProduct>(dQuery).Result as Product;
                prod3.Id.Should().Be(3);
            }
        }

        [Fact]
        public void When_Querying_For_Non_Existent_EntityType_Then_Throw_Exception()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products())
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());

                var action = new Action(() =>
                {
                    //query for a missing product based on the 'Id'
                    var dQuery = context.CreateQuery<Product>("/" + "Products(7)");

                    // this should ideally return null or thrown an exception that reflects 404.
                    Product missingProd = context.ExecuteSingleAsync<Product, IProduct>(dQuery).Result as Product;
                    //Product missingProd = dQuery.Where(p => p.Id == 7).First();
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
