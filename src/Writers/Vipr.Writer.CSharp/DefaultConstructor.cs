// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp
{
    public class DefaultConstructor : Constructor
    {
        private OdcmClass OdcmClass { get; set; }
        public Identifier Identifier { get; private set; }
        public bool IsDerived { get { return OdcmClass.Base == null; } }

        public DefaultConstructor(OdcmClass odcmClass, Identifier className)
        {
            OdcmClass = odcmClass;
            Identifier = className;
        }
    }
}
