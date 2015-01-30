// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using ODataV4TestService.Models;

namespace ODataV4TestService.Controllers
{
    public class SuppliedProductsController : ODataController
    {
        private readonly IQueryable<SuppliedProduct> _products;

        public SuppliedProductsController(IQueryable<SuppliedProduct> products)
        {
            _products = products;
        }

        [EnableQuery]
        public IQueryable<SuppliedProduct> Get()
        {
            return _products.AsQueryable();
        }

        [EnableQuery]
        public SingleResult<SuppliedProduct> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_products.Where(p => p.Id == key));
        }

        [EnableQuery]
        public SingleResult<Supplier> GetSupplier([FromODataUri] int key)
        {
            var result = _products.Where(m => m.Id == key).Select(m => m.Supplier);
            return SingleResult.Create(result);
        }
    }
}
