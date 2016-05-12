// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace Vipr.Core.CodeModel
{
    public class OdcmMethod : OdcmType
    {
        private static List<OdcmParameter> EmptyParameters = new List<OdcmParameter>();
        public OdcmClass Class { get; set; }

        public OdcmAllowedVerbs Verbs { get; set; }

        public bool IsBoundToCollection { get; set; }

        public bool IsCollection { get; set; }

        public bool IsComposable { get; set; }

        public bool IsStatic { get; set; }

        public IList<OdcmMethod> Overloads { get; private set; }

        public List<OdcmParameter> Parameters { get; private set; }

        public OdcmType ReturnType { get; set; }

        public bool IsFunction { get; set; }

        public OdcmMethod(string name, OdcmNamespace @namespace)
            : base(name, @namespace)
        {
            Parameters = new List<OdcmParameter>();
            Overloads = new List<OdcmMethod>();
        }

        public override string CanonicalName()
        {
            return MakeCanonicalName(Name, ReturnType, Parameters.ToArray());
        }
    }
}
