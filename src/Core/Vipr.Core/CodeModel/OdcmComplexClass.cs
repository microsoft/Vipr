// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Vipr.Core.CodeModel
{
    public class OdcmComplexClass : OdcmClass
    {
        public OdcmComplexClass(string name, string @namespace) :
            base(name, @namespace, OdcmClassKind.Complex)
        {
        }
    }
}
