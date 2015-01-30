// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Vipr.Core.CodeModel
{
    public class OdcmParameter : OdcmObject
    {
        public OdcmCallingConvention CallingConvention { get; set; }

        public bool IsCollection { get; set; }

        public bool IsNullable { get; set; }

        public OdcmType Type { get; set; }

        public OdcmParameter(string name)
            : base(name)
        {
        }
    }
}
