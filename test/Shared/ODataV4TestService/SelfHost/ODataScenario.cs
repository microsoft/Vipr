// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Web.OData.Builder;
using Microsoft.Practices.Unity;
using Moq;
using ODataV4TestService.Agents;
using ODataV4TestService.Handlers;
using ODataV4TestService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Extensions;
using System.Web.OData.Batch;

namespace ODataV4TestService.SelfHost
{
    public class ODataScenario : ScenarioBase<ODataScenario>
    {
        internal readonly ODataModelBuilder Builder;

        public ODataScenario()
        {
            Builder = new ODataConventionModelBuilder { Namespace = "ODataV4TestService.Models" };
        }

        public new IStartedScenario Start()
        {
            base.Start();

            EnsureRequestValidationConfigured();

            return this;
        }

        public ODataScenario WithDisabledRequestValidator()
        {
            Container.RegisterInstance(new Mock<IRequestValidationAgent>(MockBehavior.Loose).Object);

            return this;
        }

        public ODataScenario WithProducts(IQueryable<Product> products = null, IProductAgent productAgent = null)
        {
            if (!IsConfigurable) throw new InvalidOperationException("Cannot configure a started scenario.");

            if (products != null)
                Container.RegisterInstance(products.ToList());

            Builder.EntitySet<Product>("Products");

            Container.RegisterInstance(productAgent ?? new Mock<IProductAgent>(MockBehavior.Strict).Object);

            return this;
        }

        public ODataScenario WithAccounts(IList<Account> accounts = null)
        {
            if (!IsConfigurable) throw new InvalidOperationException("Cannot configure a started scenario.");

            if (accounts != null)
                Container.RegisterInstance(accounts);

            return this;
        }

        public ODataScenario WithSuppliedProducts(IQueryable<Supplier> suppliers = null)
        {
            if (!IsConfigurable) throw new InvalidOperationException("Cannot configure a started scenario.");

            if (suppliers == null) return this;

            Container.RegisterInstance(suppliers);

            Builder.EntitySet<Supplier>("Suppliers");

            var products = suppliers.SelectMany(s => s.Products).AsQueryable();

            Container.RegisterInstance(products);

            Builder.EntitySet<SuppliedProduct>("SuppliedProducts");

            return this;
        }

        private void EnsureRequestValidationConfigured()
        {
            GetHttpConfiguration().MessageHandlers.Add(
                new RequestValidationHandler(RequestValidationAgent.GetRegisteredOrNew(GetHttpConfiguration())));
        }
    }
}