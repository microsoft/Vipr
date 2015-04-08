// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Its.Recipes;
using ODataV4TestService.SelfHost;
using System;
using System.Linq;
using FluentAssertions;
using Moq;
using Microsoft.OData.Client;
using Xunit;

namespace ProxyExtensionsUnitTests
{
    public class Given_A_Method_Bound_To_EntityType : ProxyExtensionsUnitTestsBase
    {
        [Fact]
        public void When_Executing_Action_On_Client_Then_Action_Is_Called_On_Server_With_Correct_Parameters()
        {
            var rating = Any.Int();
            var productRatingAgentMock = new Mock<ODataV4TestService.Agents.IProductAgent>(MockBehavior.Strict);
            productRatingAgentMock.Setup(p => p.AddRating(It.Is<ODataV4TestService.Models.ProductRating>(r => r.ProductId == 2 && r.Rating == rating)));

            using (var scenario =
                    new ODataScenario()
                        .WithProducts(Any.Products(), productRatingAgentMock.Object)
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());

                Uri requestUri = new Uri(scenario.GetBaseAddress() + "/Products(2)/ODataV4TestService.Models.Rate");
                context.ExecuteAsync(requestUri, "POST",
                new OperationParameter[] { new BodyOperationParameter("Rating", rating) }).Wait();

                productRatingAgentMock.Verify(p => p.AddRating(It.IsAny<ODataV4TestService.Models.ProductRating>()), Times.Once);
            }
        }


        [Fact]
        public void When_Executing_Function_On_Client_Then_Function_Is_Called_On_Server_And_Returns_Expected_EntityType()
        {
            var products = Any.Products().ToList();
            var bestProduct = products.RandomElement();
            var productRatingAgentMock = new Mock<ODataV4TestService.Agents.IProductAgent>(MockBehavior.Strict);
            productRatingAgentMock.Setup(p => p.GetBest()).Returns(bestProduct);

            using (var scenario =
                    new ODataScenario()
                        .WithProducts(products.AsQueryable(), productRatingAgentMock.Object)
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());

                Uri requestUri = new Uri(scenario.GetBaseAddress() + "/Products/ODataV4TestService.Models.Best");
                var bestProducts = context.ExecuteAsync<Product>(requestUri, "GET", true,
                new OperationParameter[] { }).Result.ToList();

                bestProducts.Count().Should().Be(1);
                bestProducts[0].Id.Should().Be(bestProduct.Id);
                bestProducts[0].Name.Should().Be(bestProduct.Name);
            }
        }

        [Fact]
        public void When_Executing_Function_On_Client_Then_Function_Is_Called_On_Server_And_Returns_Expected_EntityCollection()
        {
            var products = Any.Products().ToList();
            var relatedProducts = products.RandomSequence(2).ToList();
            var productRatingAgentMock = new Mock<ODataV4TestService.Agents.IProductAgent>(MockBehavior.Strict);
            productRatingAgentMock.Setup(p => p.RelatedProducts()).Returns(relatedProducts.AsQueryable());

            using (var scenario =
                    new ODataScenario()
                        .WithProducts(products.AsQueryable(), productRatingAgentMock.Object)
                        .Start())
            {
                var context = GetDataServiceContext(scenario.GetBaseAddress());

                Uri requestUri = new Uri(scenario.GetBaseAddress() + "/Products(2)/ODataV4TestService.Models.RelatedProducts");
                var results = context.ExecuteAsync<Product>(requestUri, "GET", false,
                new OperationParameter[] {  new UriOperationParameter("start", DateTimeOffset.Now),
                                            new UriOperationParameter("end", DateTimeOffset.UtcNow) }).Result.ToList();

                results.Count.Should().Be(2);
                results[0].Id.Should().Be(relatedProducts[0].Id);
                results[1].Name.Should().Be(relatedProducts[1].Name);
            }
        }
    }
}
