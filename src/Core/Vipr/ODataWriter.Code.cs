// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vipr.Core.CodeModel;

namespace Vipr
{
    public partial class ODataWriter
    {
        private OdcmModel Model { get; set; }

        public ODataWriter(OdcmModel model)
        {
            Model = model;
        }
    }
}