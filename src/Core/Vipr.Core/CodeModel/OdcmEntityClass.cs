// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Vipr.Core.CodeModel
{
    public class OdcmEntityClass : OdcmClass
    {
        public List<OdcmProperty> Key { get; private set; }

        public OdcmEntityClass(string name, OdcmNamespace @namespace) :
            this(name, @namespace, OdcmClassKind.Entity)
        {
        }

        protected OdcmEntityClass(string name, OdcmNamespace @namespace, OdcmClassKind kind) :
            base(name, @namespace, kind)
        {
            Key = new List<OdcmProperty>();
        }
    }
}
