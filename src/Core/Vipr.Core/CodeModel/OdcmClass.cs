// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Vipr.Core.CodeModel
{
    public class OdcmClass : OdcmType
    {
        public bool IsAbstract { get; set; }

        public bool IsOpen { get; set; }

        public OdcmClass Base { get; set; }

        public IList<OdcmClass> Derived { get; private set; }

        public List<OdcmProperty> Key { get; private set; }

        public OdcmClassKind Kind { get; set; }

        public List<OdcmProperty> Properties { get; private set; }

        public List<OdcmMethod> Methods { get; private set; }

        public OdcmClass(string name, string @namespace, OdcmClassKind kind)
            : base(name, @namespace)
        {
            Kind = kind;
            Properties = new List<OdcmProperty>();
            Methods = new List<OdcmMethod>();
            Key = new List<OdcmProperty>();
            Derived = new List<OdcmClass>();
        }
    }
}
