// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Web.OData.Builder;

namespace ODataV4TestService.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        [Contained]
        public IList<PaymentInstrument> PayinPIs { get; set; }
    }
}