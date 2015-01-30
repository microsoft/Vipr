// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using ODataV4TestService.Models;
using System.Web.Http.ModelBinding;

namespace ODataV4TestService.Controllers
{
    public class SuppliersController : ODataController
    {
        private readonly IQueryable<Supplier> _suppliers;

        public SuppliersController(IQueryable<Supplier> suppliers)
        {
            _suppliers = suppliers;
        }

        [EnableQuery]
        public IQueryable<Supplier> Get()
        {
            return _suppliers.AsQueryable();
        }

        [EnableQuery]
        public SingleResult<Supplier> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_suppliers.Where(p => p.Id == key));
        }

        [EnableQuery]
        public IQueryable<SuppliedProduct> GetProducts([FromODataUri] int key)
        {
            return _suppliers.Where(p => p.Id == key).SelectMany(s => s.Products);
        }

        [HttpPost]
        [ODataRoute("Suppliers({supplierId})/Products")]
        public IHttpActionResult PostSuppliedProduct([FromODataUri] int supplierId, [FromBody]int index, [ModelBinder]SuppliedProduct item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPatch]
        public IHttpActionResult Patch([FromODataUri]int key, Delta<Supplier> changedSupplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var originalSupplier = _suppliers.SingleOrDefault(p => p.Id == key);
            if (originalSupplier == null)
            {
                return NotFound();
            }

            changedSupplier.CopyChangedValues(originalSupplier);
            return Ok();
        }
    }
}
