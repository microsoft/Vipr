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
    public class Given_An_EntityType_From_Server : ProxyExtensionsUnitTestsBase
    {
        [Fact]
        public void When_Updating_Property_On_Client_With_OnPropertyChanged_Then_That_Property_Must_Be_Updated_On_Server()
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
                // Calling 'OnPropertyChanged' for 'Category'
                // So 'Category' property must not be updated on the server side.
                prod1.OnPropertyChanged("Category");
                prod1.UpdateAsync().Wait();

                var updatedProducts = context.ExecuteAsync<Product, IProduct>(dQuery).Result;
                updatedProducts.CurrentPage.Count.Should().Be(5);
                (updatedProducts.CurrentPage[1] as Product).Category.Should().Be(newCategory, "The 'Product.Category' Property should be changed");
            }
        }

        [Fact]
        public void When_Updating_Property_On_Client_Without_OnPropertyChanged_Then_That_Property_Must_Not_Be_Updated_On_Server()
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

                Product prod2 = products.CurrentPage[2] as Product;
                string newName = Any.CompanyName();
                prod2.Name = newName;
                // Skip calling 'OnPropertyChanged' for 'Name'
                // So 'Name' property must not be updated on the server side.
                //prod2.OnPropertyChanged("Name");
                prod2.UpdateAsync().Wait();

                var updatedProducts = context.ExecuteAsync<Product, IProduct>(dQuery).Result;
                updatedProducts.CurrentPage.Count.Should().Be(5);
                (updatedProducts.CurrentPage[2] as Product).Name.Should().NotBe(newName, "The 'Product.Name' Property should not be changed");
            }
        }

        [Fact]
        public void When_Updating_ComplexType_Property_On_Client_Then_That_Property_Must_Be_Updated_On_Server()
        {
            var prods = Any.Products().ToArray();
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(prods.AsQueryable())
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                context.MergeOption = MergeOption.OverwriteChanges;
                var dQuery = context.CreateQuery<Product>("/" + "Products(3)");
                Product prod3 = context.ExecuteSingleAsync<Product, IProduct>(dQuery).Result as Product;

                var newComment = new Description { Title = Any.AlphanumericString(), Details = Any.AlphanumericString() };
                newComment.SetContainer(() => new Tuple<EntityBase, string>(prod3, "Comment"));

                prod3.Comment = newComment;
                prod3.OnPropertyChanged("Comment");
                prod3.UpdateAsync().Wait();

                Product updatedProd3 = context.ExecuteSingleAsync<Product, IProduct>(dQuery).Result as Product;
                updatedProd3.Comment.Details.Should().Be(newComment.Details);
                updatedProd3.Comment.Title.Should().Be(newComment.Title);
            }
        }


        [Fact]
        public void When_Updates_To_Properties_Are_Delayed_Then_They_Must_Be_Updated_After_Calling_SaveChanges()
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
                prod1.OnPropertyChanged("Category");
                prod1.UpdateAsync(true).Wait();

                Product prod2 = products.CurrentPage[2] as Product;
                string newName = Any.CompanyName();
                prod2.Name = newName;
                prod2.OnPropertyChanged("Name");
                prod2.UpdateAsync(true).Wait();

                Product prod3 = products.CurrentPage[3] as Product;
                decimal newPrice = Any.Decimal();
                prod3.Price = newPrice;
                prod3.OnPropertyChanged("Price");
                prod3.UpdateAsync(true).Wait();

                var response = context.SaveChangesAsync().Result;
                response.Count().Should().Be(3);

                // Make sure everything is updated on the server
                var updatedProducts = context.ExecuteAsync<Product, IProduct>(dQuery).Result;
                updatedProducts.CurrentPage.Count.Should().Be(5);
                (updatedProducts.CurrentPage[1] as Product).Category.Should().Be(newCategory);
                (updatedProducts.CurrentPage[2] as Product).Name.Should().Be(newName);
                (updatedProducts.CurrentPage[3] as Product).Price.Should().Be(newPrice);
            }
        }

        [Fact]
        public void When_Deleting_EntityType_On_Client_Then_It_Should_Must_Be_Deleted_On_Server()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products())
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                var dQuery = context.CreateQuery<Product>("/" + "Products");
                var products = context.ExecuteAsync<Product, IProduct>(dQuery).Result;

                Product prod1 = products.CurrentPage[1] as Product;
                prod1.Id.Should().Be(2);
                prod1.DeleteAsync().Wait();

                var updatedProducts = context.ExecuteAsync<Product, IProduct>(dQuery).Result;
                updatedProducts.CurrentPage.Count.Should().Be(4);
                (updatedProducts.CurrentPage[1] as Product).Id.Should().Be(3);
            }
        }

        [Fact]
        public void When_Updating_Binary_Primitive_Property_On_Client_Then_That_Property_Must_Be_Updated_On_Server()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithSuppliedProducts(Any.Suppliers())
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                context.MergeOption = MergeOption.OverwriteChanges;
                var dQuery = context.CreateQuery<Supplier>("/" + "Suppliers");
                var suppliers = context.ExecuteAsync<Supplier, ISupplier>(dQuery).Result;
                suppliers.CurrentPage.Count.Should().Be(5);

                var supplier = suppliers.CurrentPage[2] as Supplier;
                // make sure that the binary primitive property is serialized
                supplier.Blob.Should().NotBeNullOrEmpty();

                // lets update the binary primitive property
                var newBlob = Any.Sequence<byte>(x => Any.Byte()).ToArray();
                supplier.Blob = newBlob;
                supplier.OnPropertyChanged("Blob");
                supplier.UpdateAsync().Wait();

                var updatedsuppliers = context.ExecuteAsync<Supplier, ISupplier>(dQuery).Result;
                updatedsuppliers.CurrentPage.Count.Should().Be(5);
                updatedsuppliers.CurrentPage[2].Blob.Should().BeEquivalentTo(newBlob);
            }
        }

        [Fact(Skip = "Issue #3 https://github.com/Microsoft/vipr/issues/3")]
        public void When_Updating_GeoSpatial_Primitive_Property_On_Client_Then_That_Property_Must_Be_Updated_On_Server()
        {
            using (var scenario =
                    new ODataScenario()
                        .WithSuppliedProducts(Any.Suppliers())
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());
                context.MergeOption = MergeOption.OverwriteChanges;
                var dQuery = context.CreateQuery<Supplier>("/" + "Suppliers");
                var suppliers = context.ExecuteAsync<Supplier, ISupplier>(dQuery).Result;
                suppliers.CurrentPage.Count.Should().Be(5);

                var supplier = suppliers.CurrentPage[2] as Supplier;
                // make sure that the binary primitive property is serialized
                supplier.Location.Should().NotBeNull();

                // lets update the geospatial primitive property
                var newLocation = GeographyPoint.Create(Any.Double(-90, 90), Any.Double(-180, 180));
                supplier.Location = newLocation;
                supplier.OnPropertyChanged("Location");

                // Bug - Location is a property of primitive type - "Microsoft.Spatial.GeographyPoint".
                // Its underlying type is "Microsoft.Data.Spatial.GeographyPointImplementation".
                // In the ASP.Net stack when deserializing the 'Location' property InvalidCastException is thrown.
                supplier.UpdateAsync().Wait();

                var updatedsuppliers = context.ExecuteAsync<Supplier, ISupplier>(dQuery).Result;
                updatedsuppliers.CurrentPage.Count.Should().Be(5);
                updatedsuppliers.CurrentPage[2].Location.Should().Be(newLocation);
            }
        }
    }
}
