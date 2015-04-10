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
using ProxyExtensionsUnitTests.Extensions;
using Xunit;

namespace ProxyExtensionsUnitTests
{
    public class Given_An_EntityType_Created_On_Client : ProxyExtensionsUnitTestsBase
    {
        [Fact]
        public void When_Adding_EntityType_With_All_Properties_Then_It_Must_Be_Added_To_Server_With_All_Properties()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products())
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                context.MergeOption = MergeOption.OverwriteChanges;
                var dQuery = context.CreateQuery<Product>("/" + "Products");

                var products = context.ExecuteAsync<Product, IProduct>(dQuery).Result;
                products.CurrentPage.Count.Should().Be(5);

                var newProduct = new Product();
                newProduct.Id = 6;
                newProduct.Name = Any.CompanyName();
                newProduct.Price = Any.Decimal();
                newProduct.Category = Any.AlphanumericString();
                newProduct.CallOnPropertyChanged("Id");
                newProduct.CallOnPropertyChanged("Name");
                newProduct.CallOnPropertyChanged("Price");
                newProduct.CallOnPropertyChanged("Category");

                context.AddObject("Products", newProduct);
                context.SaveChangesAsync().Wait();

                var updatedProducts = context.ExecuteAsync<Product, IProduct>(dQuery).Result;
                updatedProducts.CurrentPage.Count.Should().Be(6);
                updatedProducts.CurrentPage[5].Should().BeSameAs(newProduct);
            }
        }


        [Fact]
        public void When_Adding_EntityType_With_Partial_Properties_Then_It_Must_Be_Added_To_Server_With_Partial_Properties()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products())
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                var dQuery = context.CreateQuery<Product>("/" + "Products");

                var products = context.ExecuteAsync<Product, IProduct>(dQuery).Result;
                products.CurrentPage.Count.Should().Be(5);

                var newProduct = new Product();
                string newName = Any.CompanyName();
                newProduct.Id = 6;
                newProduct.Name = newName;
                newProduct.Price = Any.Decimal();
                newProduct.Category = Any.AlphanumericString();

                // calling 'OnPropertyChanged' only for 'Id' and 'Name' properties
                newProduct.CallOnPropertyChanged("Id");
                newProduct.CallOnPropertyChanged("Name");

                context.AddObject("Products", newProduct);
                context.SaveChangesAsync().Wait();

                var updatedProducts = context.ExecuteAsync<Product, IProduct>(dQuery).Result;
                updatedProducts.CurrentPage.Count.Should().Be(6);

                // the 'Id' and 'Name' properties must be set
                updatedProducts.CurrentPage[5].Id.Should().Be(6);
                updatedProducts.CurrentPage[5].Name.Should().Be(newName);

                // the 'Price' and 'Category' properties must not be set
                updatedProducts.CurrentPage[5].Price.Should().Be(0);
                updatedProducts.CurrentPage[5].Category.Should().BeNull();
            }
        }
    }
}
