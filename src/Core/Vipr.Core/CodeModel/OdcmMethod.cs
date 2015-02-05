// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace Vipr.Core.CodeModel
{
    public class OdcmMethod : OdcmAnnotatedObject
    {
        public OdcmClass Class { get; set; }

        public OdcmAllowedVerbs Verbs { get; set; }

        public bool IsBoundToCollection { get; set; }

        public bool IsCollection { get; set; }

        public bool IsComposable { get; set; }

        public bool IsStatic { get; set; }

        public List<OdcmParameter> Parameters { get; private set; }

        public OdcmType ReturnType { get; set; }

        public OdcmMethod(string name)
            : base(name)
        {
            Parameters = new List<OdcmParameter>();
        }

        public override string CanonicalName()
        {
            return MakeCanonicalName(Name, ReturnType, Parameters.ToArray());
        }
    }
}
