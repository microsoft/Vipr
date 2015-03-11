// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Vipr.Core.CodeModel
{
    public abstract class OdcmType : OdcmObject
    {
        public OdcmNamespace Namespace { get; set; }

        protected OdcmType(string name, OdcmNamespace @namespace)
            : base(name)
        {
            Namespace = @namespace;
        }

        public string FullName
        {
            get { return Namespace.Name + "." + Name; }
        }

        public override string CanonicalName()
        {
            return MakeCanonicalName(base.CanonicalName(), Namespace);
        }
    }
}
