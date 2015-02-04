// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Vipr.Core.CodeModel
{
    public class OdcmEnum : OdcmType
    {
        public OdcmPrimitiveType UnderlyingType { get; set; }

        public List<OdcmEnumMember> Members { get; private set; }

        public bool IsFlags { get; set; }

        public OdcmEnum(string name, string @namespace)
            : base(name, @namespace)
        {
            Members = new List<OdcmEnumMember>();
        }
    }
}
