// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Vipr.Core.CodeModel
{
    public abstract class OdcmType : OdcmAnnotatedObject
    {
        public string Namespace { get; set; }

        public OdcmType(string name, string @namespace)
            : base(name)
        {
            Namespace = @namespace;
        }

        public string FullName
        {
            get { return Namespace + "." + Name; }
        }
    }
}
