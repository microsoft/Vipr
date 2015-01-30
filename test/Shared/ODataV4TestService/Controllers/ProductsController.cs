// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using ODataV4TestService.Agents;
using ODataV4TestService.Models;

namespace ODataV4TestService.Controllers
{
    public class ProductsController : ODataController
    {
        private readonly List<Product> _products;
        private readonly IProductAgent _productAgent;

        public ProductsController(List<Product> products, IProductAgent productAgent)
        {
            _products = products;
            _productAgent = productAgent;
        }

        [EnableQuery(PageSize = 10)]
        public IQueryable<Product> Get()
        {
            return _products.AsQueryable();
        }

        [EnableQuery]
        public IHttpActionResult Get([FromODataUri] int key)
        {
            var product = _products.SingleOrDefault(p => p.Id == key);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(SingleResult.Create(_products.Where(p => p.Id == key).AsQueryable()));
        }

        [HttpPost]
        public IHttpActionResult Post(Product item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _products.Add(item);
            return Created(item);
        }

        [HttpPost]
        public IHttpActionResult Rate([FromODataUri] int key, ODataActionParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var rating = (int)parameters["Rating"];

            _productAgent.AddRating(new ProductRating { ProductId = key, Rating = rating });

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPatch]
        public IHttpActionResult Patch([FromODataUri]int key, Delta<Product> changedProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var originalProduct = _products.SingleOrDefault(p => p.Id == key);
            if (originalProduct == null)
            {
                return NotFound();
            }

            changedProduct.CopyChangedValues(originalProduct);
            return StatusCode(HttpStatusCode.OK);
        }

        [HttpDelete]
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            var delProduct = _products.SingleOrDefault(p => p.Id == key);
            if (delProduct == null)
            {
                return NotFound();
            }

            _products.Remove(delProduct);
            return StatusCode(HttpStatusCode.OK);
        }

        [HttpGet]
        public Product Best()
        {
            var product = _productAgent.GetBest();
            return product;
        }

        //[HttpGet]
        //[ODataRoute("GetSalesTaxRate(PostalCode={postalCode})")]
        //public IHttpActionResult GetSalesTaxRate([FromODataUri] int postalCode)
        //{
        //    var rate = _productAgent.GetSalesTaxRate(postalCode);
        //    return Ok(rate);
        //}

        [HttpGet]
        public IQueryable<Product> RelatedProducts(Product supplier, DateTimeOffset start, DateTimeOffset end)
        {
            return _productAgent.RelatedProducts().AsQueryable();
        }
    }
}
