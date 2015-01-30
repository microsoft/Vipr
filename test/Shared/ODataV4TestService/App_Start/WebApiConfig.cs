// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using Microsoft.OData.Edm;
using ODataV4TestService.Agents;
using ODataV4TestService.Handlers;
using ODataV4TestService.Models;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;
using System.Web.OData.Batch;
using System.Web.Http.Controllers;
using System.Net.Http;
using Microsoft.Data.OData;

namespace ODataV4TestService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration httpConfiguration)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.Namespace = "ODataV4TestService.Models";
            builder.EntitySet<Product>("Products");
            builder.EntitySet<SuppliedProduct>("SuppliedProducts");
            builder.EntitySet<Supplier>("Suppliers");
            builder.EntitySet<Product>("OtherProducts");

            builder.ComplexType<Description>();
            builder.EntityType<Product>()
                .Action("Rate")
                .Parameter<int>("Rating");

            builder.EntityType<Product>().Collection
                .Function("Best")
                .ReturnsCollectionFromEntitySet<Product>("Products");

            var funcConfig = builder
                .EntityType<Product>()
                .Function("RelatedProducts")
                .SetBindingParameter("product", builder.GetTypeConfigurationOrNull(typeof(Product)))
                //.AddParameter("start", new PrimitiveTypeConfiguration(builder, builder.GetTypeConfigurationOrNull(typeof(DateTimeOffset)), typeof(DateTimeOffset)).
                .ReturnsCollectionFromEntitySet<Product>("Products");

            funcConfig
                .Parameter<DateTimeOffset>("start");

            funcConfig
                .Parameter<DateTimeOffset>("end");

            //builder.Function("GetSalesTaxRate")
            //    .Returns<double>()
            //    .Parameter<int>("PostalCode");

            builder.EntitySet<Account>("Accounts");

            builder.EntityType<PaymentInstrument>()
                .Collection
                .Function("GetCount")
                .Returns<int>()
                .Parameter<string>("NameContains");

            var model = builder.GetEdmModel();

            var conventions = ODataRoutingConventions.CreateDefault();
            conventions.Insert(0, new AttributeRoutingConvention(model, httpConfiguration));

            var server = new BatchServer(httpConfiguration);

            httpConfiguration.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: null,
                model: model,
                pathHandler: new DefaultODataPathHandler(),
                routingConventions: conventions,
                batchHandler: new DefaultODataBatchHandler(server));

            httpConfiguration.MessageHandlers.Add(new TracingMessageHandler());
        }
    }
}
