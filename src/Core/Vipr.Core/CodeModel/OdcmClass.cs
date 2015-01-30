// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Vipr.Core.CodeModel
{
    public class OdcmClass : OdcmType
    {
        public bool IsAbstract { get; set; }

        public List<OdcmField> Key { get; private set; }

        public OdcmClassKind Kind { get; private set; }

        public List<OdcmField> Fields { get; private set; }

        public List<OdcmProperty> Properties { get; private set; }

        public List<OdcmMethod> Methods { get; private set; }

        public OdcmClass(string name, string @namespace, OdcmClassKind kind)
            : base(name, @namespace)
        {
            Kind = kind;
            Fields = new List<OdcmField>();
            Properties = new List<OdcmProperty>();
            Methods = new List<OdcmMethod>();
            Key = new List<OdcmField>();
        }

        public override string AsReference()
        {
            throw new System.NotImplementedException();
        }
    }
}
