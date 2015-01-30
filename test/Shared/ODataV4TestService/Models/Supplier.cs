// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODataV4TestService.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<SuppliedProduct> Products { get; set; }
        public byte[] Blob { get; set; }
        public GeographyPoint Location { get; set; }
    }
}