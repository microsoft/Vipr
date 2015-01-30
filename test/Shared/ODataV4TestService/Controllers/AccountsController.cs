// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using ODataV4TestService.Models;

namespace ODataV4TestService.Controllers
{
    public class AccountsController : ODataController
    {
        private static IList<Account> s_accounts;

        public AccountsController(IList<Account> accounts)
        {
            s_accounts = accounts;
        }

        [EnableQuery]
        public IQueryable<Account> Get()
        {
            return s_accounts.AsQueryable();
        }

        [EnableQuery]
        public SingleResult<Account> Get([FromODataUri] int key)
        {
            return SingleResult.Create(s_accounts.AsQueryable().Where(a => a.AccountId == key));
        }

        // GET ~/Accounts(100)/PayinPIs         
        [EnableQuery]
        public IHttpActionResult GetPayinPIs(int key)
        {
            var payinPIs = s_accounts.Single(a => a.AccountId == key).PayinPIs;
            return Ok(payinPIs);
        }


        [EnableQuery]
        [ODataRoute("Accounts({accountId})/PayinPIs({paymentInstrumentId})")]
        public IHttpActionResult GetPayinPIs([FromODataUri]int accountId, [FromODataUri]int paymentInstrumentId)
        {
            var payinPIs = s_accounts.Single(a => a.AccountId == accountId).PayinPIs;
            var payinPI = payinPIs.Single(pi => pi.PaymentInstrumentId == paymentInstrumentId);
            return Ok(payinPI);
        }
        //// PUT ~/Accounts(100)/PayinPIs(101)         
        //[ODataRoute("Accounts({accountId})/PayinPIs({paymentInstrumentId})")]
        //public IHttpActionResult PutToPayinPI(int accountId, int paymentInstrumentId, [FromBody]PaymentInstrument paymentInstrument)
        //{
        //    var account = _accounts.Single(a => a.AccountId == accountId);
        //    var originalPi = account.PayinPIs.Single(p => p.PaymentInstrumentId == paymentInstrumentId);
        //    originalPi.FriendlyName = paymentInstrument.FriendlyName;
        //    return Ok(paymentInstrument);
        //}

        //// DELETE ~/Accounts(100)/PayinPIs(101)         
        //[ODataRoute("Accounts({accountId})/PayinPIs({paymentInstrumentId})")]
        //public IHttpActionResult DeletePayinPIFromAccount(int accountId, int paymentInstrumentId)
        //{
        //    var account = _accounts.Single(a => a.AccountId == accountId);
        //    var originalPi = account.PayinPIs.Single(p => p.PaymentInstrumentId == paymentInstrumentId);
        //    if (account.PayinPIs.Remove(originalPi))
        //    {
        //        return StatusCode(HttpStatusCode.NoContent);
        //    }
        //    else
        //    {
        //        return StatusCode(HttpStatusCode.InternalServerError);
        //    }
        //}

        // GET ~/Accounts(100)/PayinPIs/Namespace.GetCount() 
        //[ODataRoute("Accounts({accountId})/PayinPIs/ODataV4TestService.Models.GetCount(NameContains={name})")]
        //public IHttpActionResult GetPayinPIsCountWhoseNameContainsGivenValue(int accountId, [FromODataUri]string name)
        //{
        //    var account = _accounts.Single(a => a.AccountId == accountId);
        //    var count = account.PayinPIs.Count(pi => pi.FriendlyName.Contains(name));
        //    return Ok(count);
        //}     
    }
}
