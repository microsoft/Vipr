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
using Microsoft.OData.Core;
using Xunit;

namespace ProxyExtensionsUnitTests
{
    public class Given_A_ReadOnlyQueryableSet : ProxyExtensionsUnitTestsBase
    {
        [Fact]
        public void When_Using_OrderBy_Clause_Then_Returned_Sequence_Is_Sorted_Ascending()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products(8))
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                var dQuery = context.CreateQuery<Product>("/" + "Products");
                IReadOnlyQueryableSet<IProduct> readOnlySet = new ReadOnlyQueryableSet<IProduct>(dQuery, context);

                //Query directly
                var products = readOnlySet.OrderBy(p => p.Name).ExecuteAsync().Result;
                products.CurrentPage.Count.Should().Be(8);
                products.CurrentPage.Should().BeInAscendingOrder(p => p.Name);

                //Query using linq
                var linqQuery = from prod in readOnlySet
                                orderby prod.Category
                                select prod;
                products = linqQuery.ExecuteAsync().Result;
                products.CurrentPage.Count.Should().Be(8);
                products.CurrentPage.Should().BeInAscendingOrder(p => p.Category);
            }
        }

        [Fact]
        public void When_Query_Matches_Multiple_EntityTypes_Then_Using_ExecuteSingleAsync_Must_Fail()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products(8))
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                var dQuery = context.CreateQuery<Product>("/" + "Products");
                IReadOnlyQueryableSet<IProduct> readOnlySet = new ReadOnlyQueryableSet<IProduct>(dQuery, context);

                //Query using linq
                var linqQuery = from prod in readOnlySet
                                orderby prod.Category
                                select prod;

                var action = new Action(() =>
                {
                    var product = linqQuery.ExecuteSingleAsync().Result;
                });

                action.ShouldThrow<AggregateException>().
                    WithInnerException<InvalidOperationException>().
                    WithInnerMessage("Sequence contains more than one element");
            }
        }

        [Fact]
        public void When_Query_Matches_One_EntityType_Then_Using_ExecuteSingleAsync_Must_Succeed()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products(8))
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                var dQuery = context.CreateQuery<Product>("/" + "Products");
                IReadOnlyQueryableSet<IProduct> readOnlySet = new ReadOnlyQueryableSet<IProduct>(dQuery, context);

                //Query using linq
                var linqQuery = from prod in readOnlySet
                                where prod.Id == 2
                                select prod;

                var product = linqQuery.ExecuteSingleAsync().Result;
                product.Id.Should().Be(2);
            }
        }

        [Fact(Skip = "Issue #4 https://github.com/Microsoft/vipr/issues/4")]
        public void When_Using_Expand_Clause_On_EntityType_Interface_Then_Resulting_Sequence_Must_Be_Expanded()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithSuppliedProducts(Any.Suppliers())
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());

                var dQuery = context.CreateQuery<SuppliedProduct>("/" + "SuppliedProducts");
                IReadOnlyQueryableSet<ISuppliedProduct> productSet = new ReadOnlyQueryableSet<ISuppliedProduct>(dQuery, context);

                var products = productSet.Expand(p => p.Supplier)
                    .ExecuteAsync().Result;
                products.CurrentPage.Count.Should().Be(25);
                products.CurrentPage.Should().OnlyContain(p => p.Supplier != null);
            }
        }

        [Fact]
        public void When_Using_Filter_Clause_On_Expanded_Navigation_Property_Then_Returned_Sequence_Is_Filtered()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithSuppliedProducts(Any.Suppliers())
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());

                var dQuery = context.CreateQuery<SuppliedProduct>("/" + "SuppliedProducts");
                IReadOnlyQueryableSet<ISuppliedProduct> productSet = new ReadOnlyQueryableSet<ISuppliedProduct>(dQuery, context);
                var products = productSet.Expand(p => (p as SuppliedProduct).Supplier).
                    Where(p => p.Supplier.Id == 2).
                    ExecuteAsync().Result;
                products.CurrentPage.Count.Should().Be(5);
            }
        }

        [Fact]
        public void When_Using_Orderby_Clause_Before_Projections_Then_Returned_Sequence_Contains_ProjectedTypes_Sorted_Ascending()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithSuppliedProducts(Any.Suppliers())
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());

                var dQuery = context.CreateQuery<SuppliedProduct>("/" + "SuppliedProducts");
                IReadOnlyQueryableSet<SuppliedProduct> prodSet = new ReadOnlyQueryableSet<SuppliedProduct>(dQuery, context);

                var linqQuery =
                           from prod in prodSet
                           where prod.Id < 400
                           orderby prod.Name
                           select new { supplier = prod.Supplier, prod.Name } into x
                           select x;

                var products = linqQuery.ExecuteAsync().Result;
                products.CurrentPage.Count.Should().Be(15);
                products.CurrentPage.Should().BeInAscendingOrder(a => a.Name);
            }
        }

        [Fact]
        public void When_Using_Orderby_Clause_After_Projections_Then_Throw_NotSupportedException()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithSuppliedProducts(Any.Suppliers())
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());

                var dQuery = context.CreateQuery<SuppliedProduct>("/" + "SuppliedProducts");
                IReadOnlyQueryableSet<SuppliedProduct> prodSet = new ReadOnlyQueryableSet<SuppliedProduct>(dQuery, context);

                var linqQuery =
                           from prod in prodSet
                           where prod.Id < 400
                           select new { supplier = prod.Supplier, prod } into x
                           orderby x.supplier.Name
                           select x;

                var action = new Action(() =>
                {
                    var products = linqQuery.ExecuteAsync().Result;
                });

                action.ShouldThrow<AggregateException>().
                    WithInnerException<NotSupportedException>().
                    WithInnerMessage("The orderby query option cannot be specified after the select query option.");
            }
        }



        [Fact]
        public void When_Using_OrderByDescending_Clause_Then_Returned_Sequence_Is_Sorted_Descending()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products(8))
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                var dQuery = context.CreateQuery<Product>("/" + "Products");
                IReadOnlyQueryableSet<Product> readOnlySet = new ReadOnlyQueryableSet<Product>(dQuery, context);

                //Query directly
                var products = readOnlySet.OrderByDescending(p => p.Name).ExecuteAsync().Result;
                products.CurrentPage.Count.Should().Be(8);
                products.CurrentPage.Should().BeInDescendingOrder(p => p.Name);

                //Query using linq
                var linqQuery = from prod in readOnlySet
                                orderby prod.Price descending
                                select prod;


                products = linqQuery.ExecuteAsync().Result;
                products.CurrentPage.Count.Should().Be(8);
                products.CurrentPage.Should().BeInDescendingOrder(p => p.Price);
            }
        }

        [Fact]
        public void When_Using_Filter_Clause_Then_Returned_Sequence_Is_Filtered()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products(14))
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                var dQuery = context.CreateQuery<Product>("/" + "Products");
                IReadOnlyQueryableSet<Product> readOnlySet = new ReadOnlyQueryableSet<Product>(dQuery, context);

                //Query directly
                var products = readOnlySet.Where(p => p.Id <= 7).ExecuteAsync().Result;
                products.CurrentPage.Count.Should().Be(7);

                //Query using linq
                var linqQuery = from prod in readOnlySet
                                where prod.Id <= 9
                                select prod;
                products = linqQuery.ExecuteAsync().Result;
                products.CurrentPage.Count.Should().Be(9);
            }
        }

        [Fact]
        public void When_Using_Projections_Then_Returned_Sequence_Contains_ProjectedTypes()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products(8))
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                var dQuery = context.CreateQuery<Product>("/" + "Products");
                IReadOnlyQueryableSet<Product> readOnlySet = new ReadOnlyQueryableSet<Product>(dQuery, context);

                //Query directly
                var prodNames = readOnlySet.Select(p => new { p.Name, p.Price }).ExecuteAsync().Result;
                prodNames.CurrentPage.Count.Should().Be(8);

                //Query using linq
                var linqQuery = from prod in readOnlySet
                                select new { prod.Name, prod.Price };
                prodNames = linqQuery.ExecuteAsync().Result;
                prodNames.CurrentPage.Count.Should().Be(8);
            }
        }

        [Fact]
        public void When_SkipCount_IsWrong_Then_Throw_ODataErrorException()
        {
            int skipCount = -3;
            int productsCount = 8;
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products(productsCount))
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                var dQuery = context.CreateQuery<Product>("/" + "Products");
                IReadOnlyQueryableSet<IProduct> readOnlySet = new ReadOnlyQueryableSet<IProduct>(dQuery, context);

                var action = new Action(() =>
                {
                    var products = readOnlySet.Skip(skipCount).ExecuteAsync().Result;
                });

                action.ShouldThrow<AggregateException>().WithInnerException<ODataErrorException>();
            }
        }

        [Fact]
        public void When_SkipCount_LesserThan_EntitySet_Count_Then_Return_EntitySet_With_TopResults_Skipped()
        {
            int skipCount = 3;
            int productsCount = 8;
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products(productsCount))
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                var dQuery = context.CreateQuery<Product>("/" + "Products");
                IReadOnlyQueryableSet<IProduct> readOnlySet = new ReadOnlyQueryableSet<IProduct>(dQuery, context);

                //skip the top 3 results
                var products = readOnlySet.Skip(skipCount).ExecuteAsync().Result;
                products.CurrentPage.Count.Should().Be(productsCount - skipCount);
                (products.CurrentPage[0] as Product).Id.Should().Be(4);
            }
        }

        [Fact]
        public void When_SkipCount_GreaterThan_EntitySet_Count_Then_Return_EntitySet_With_Zero_Elements()
        {
            int skipCount = 12;
            int productsCount = 8;
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products(productsCount))
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                var dQuery = context.CreateQuery<Product>("/" + "Products");
                IReadOnlyQueryableSet<IProduct> readOnlySet = new ReadOnlyQueryableSet<IProduct>(dQuery, context);

                //skip the top 12 results
                var products = readOnlySet.Skip(skipCount).ExecuteAsync().Result;
                products.CurrentPage.Count.Should().Be(0);
            }
        }

        [Fact]
        public void When_SkipCount_EqualsTo_EntitySet_Count_Then_Return_EntitySet_With_Zero_Elements()
        {
            int skipCount = 8;
            int productsCount = 8;
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products(productsCount))
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                var dQuery = context.CreateQuery<Product>("/" + "Products");
                IReadOnlyQueryableSet<IProduct> readOnlySet = new ReadOnlyQueryableSet<IProduct>(dQuery, context);

                //skip the top 8 results
                var products = readOnlySet.Skip(skipCount).ExecuteAsync().Result;
                products.CurrentPage.Count.Should().Be(0);
            }
        }

        [Fact]
        public void When_TakeCount_LesserThan_EntitySet_Count_Then_Return_EntitySet_With_TakeCount_TopResults()
        {
            int takeCount = 6;
            int productsCount = 8;
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products(productsCount))
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                var dQuery = context.CreateQuery<Product>("/" + "Products");
                IReadOnlyQueryableSet<IProduct> readOnlySet = new ReadOnlyQueryableSet<IProduct>(dQuery, context);

                //take the top 6 results
                var products = readOnlySet.Take(takeCount).ExecuteAsync().Result;
                products.CurrentPage.Count.Should().Be(takeCount);
                (products.CurrentPage[5] as Product).Id.Should().Be(6);
            }
        }

        [Fact]
        public void When_TakeCount_IsWrong_Then_Throw_ODataErrorException()
        {
            int takeCount = -6;
            int productsCount = 8;
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products(productsCount))
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                var dQuery = context.CreateQuery<Product>("/" + "Products");
                IReadOnlyQueryableSet<IProduct> readOnlySet = new ReadOnlyQueryableSet<IProduct>(dQuery, context);

                var action = new Action(() =>
                {
                    var products = readOnlySet.Skip(takeCount).ExecuteAsync().Result;
                });

                action.ShouldThrow<AggregateException>().WithInnerException<ODataErrorException>();
            }
        }

        [Fact]
        public void When_TakeCount_GreaterThan_EntitySet_Count_Then_Return_EntitySet_With_EntitySet_Count()
        {
            int takeCount = 10;
            int productsCount = 8;
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products(productsCount))
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                var dQuery = context.CreateQuery<Product>("/" + "Products");
                IReadOnlyQueryableSet<IProduct> readOnlySet = new ReadOnlyQueryableSet<IProduct>(dQuery, context);

                //take the top 10 results
                var products = readOnlySet.Take(takeCount).ExecuteAsync().Result;
                products.CurrentPage.Count.Should().Be(productsCount);
                (products.CurrentPage[7] as Product).Id.Should().Be(8);
            }
        }

        [Fact]
        public void When_TakeCount_EqualsTo_EntitySet_Count_Then_Return_EntitySet_With_TakeCount()
        {
            int takeCount = 8;
            int productsCount = 8;
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products(productsCount))
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                var dQuery = context.CreateQuery<Product>("/" + "Products");
                IReadOnlyQueryableSet<IProduct> readOnlySet = new ReadOnlyQueryableSet<IProduct>(dQuery, context);

                //take the top 10 results
                var products = readOnlySet.Take(takeCount).ExecuteAsync().Result;
                products.CurrentPage.Count.Should().Be(takeCount);
                (products.CurrentPage[7] as Product).Id.Should().Be(8);
            }
        }
    }
}
