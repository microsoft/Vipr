// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Vipr.Core.CodeModel
{
    public class OdcmTypeDefinition : OdcmType
    {
        public bool IsOpen { get; set; }

        public OdcmType BaseType { get; set; }

        public bool IsNullable { get; set; }

        public OdcmTypeDefinition(string name, OdcmNamespace @namespace)
            : base(name, @namespace)
        {
        }
    }
}