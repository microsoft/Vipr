// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace Vipr.Core.CodeModel
{
    public class OdcmNestedType : OdcmType
    {
        public readonly static string Collection = "Collection";

        public List<OdcmType> InnerTypes { get; private set; }

        public OdcmNestedType(string name, params OdcmType[] innerTypes)
            : base(name, Global)
        {
            InnerTypes = new List<OdcmType>(innerTypes);
        }

        public override string CanonicalName()
        {
            return MakeCanonicalName(Name, Namespace, InnerTypes.ToArray());
        }
    }
}
