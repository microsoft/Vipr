// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Vipr.Core.CodeModel
{
    public class OdcmMediaClass : OdcmEntityClass
    {
        public OdcmMediaClass(string name, OdcmNamespace @namespace)
            : base(name, @namespace, OdcmClassKind.MediaEntity)
        {
        }
    }
}
