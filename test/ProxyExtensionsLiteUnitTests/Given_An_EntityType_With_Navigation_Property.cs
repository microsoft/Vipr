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
    public class Given_An_EntityType_With_Navigation_Property : ProxyExtensionsUnitTestsBase
    {
        [Fact]
        public void When_Querying_For_NavigationProperty_Then_Return_Single_Expected_EntityType()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithSuppliedProducts(Any.Suppliers())
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());

                var dQuery = context.CreateQuery<SuppliedProduct>("/" + "SuppliedProducts");
                IReadOnlyQueryableSet<SuppliedProduct> productSet = new ReadOnlyQueryableSet<SuppliedProduct>(dQuery, context);
                var products = productSet.Expand(p => p.Supplier).ExecuteAsync().Result;
                products.CurrentPage.Count.Should().Be(25);

                var suppliedProduct = products.CurrentPage.RandomElement() as SuppliedProduct;
                string supplierPath = suppliedProduct.GetPath("Supplier");
                var supplierQuery = context.CreateQuery<Supplier>(supplierPath);

                IReadOnlyQueryableSet<Supplier> supplierSet = new ReadOnlyQueryableSet<Supplier>(supplierQuery, context);
                var supplier = supplierSet.ExecuteSingleAsync().Result;
                suppliedProduct.Supplier.Id.Should().Be(supplier.Id);
            }
        }

        [Fact]
        public void When_Querying_For_Series_Of_NavigationProperties_Then_Each_Query_Return_Expected_EntityType_Or_Collection()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithSuppliedProducts(Any.Suppliers())
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());

                var dQuery = context.CreateQuery<SuppliedProduct>("/" + "SuppliedProducts");
                IReadOnlyQueryableSet<SuppliedProduct> productSet = new ReadOnlyQueryableSet<SuppliedProduct>(dQuery, context);
                var products = productSet.Expand(p => p.Supplier).ExecuteAsync().Result;
                products.CurrentPage.Count.Should().Be(25);

                var suppliedProduct = products.CurrentPage.RandomElement() as SuppliedProduct;
                string supplierPath = suppliedProduct.GetPath("Supplier");
                var supplierQuery = context.CreateQuery<Supplier>(supplierPath);
                IReadOnlyQueryableSet<Supplier> supplierSet = new ReadOnlyQueryableSet<Supplier>(supplierQuery, context);
                //TODO - Unable to expand a collection
                var supplier = supplierSet.Expand(s => s.Products).ExecuteSingleAsync().Result;
                suppliedProduct.Supplier.Id.Should().Be(supplier.Id);

                // Hence querying for the SuppliedProducts
                string productsPath = supplier.GetPath("Products");
                dQuery = context.CreateQuery<SuppliedProduct>(productsPath);
                productSet = new ReadOnlyQueryableSet<SuppliedProduct>(dQuery, context);
                products = productSet.ExecuteAsync().Result;
                products.CurrentPage.Count.Should().Be(5);
                products.CurrentPage.Should().Contain(p => p.Id == suppliedProduct.Id);
            }
        }

        [Fact]
        public void When_Querying_For_NavigationPropertyCollection_Then_Return_Expected_EntityTypeCollection()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithSuppliedProducts(Any.Suppliers())
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());

                var dQuery = context.CreateQuery<Supplier>("/" + "Suppliers");
                IReadOnlyQueryableSet<Supplier> supplierSet = new ReadOnlyQueryableSet<Supplier>(dQuery, context);
                var suppliers = supplierSet.ExecuteAsync().Result;
                suppliers.CurrentPage.Count.Should().Be(5);

                var supplier = suppliers.CurrentPage.RandomElement();
                string productsPath = supplier.GetPath("Products");
                var dQueryProds = context.CreateQuery<SuppliedProduct>(productsPath);
                var productSet = new ReadOnlyQueryableSet<SuppliedProduct>(dQueryProds, context);

                var products = productSet.ExecuteAsync().Result;
                products.CurrentPage.Count.Should().Be(5);
                products.CurrentPage.Should().Contain(p => p.SupplierId == supplier.Id);
            }
        }
    }
}
