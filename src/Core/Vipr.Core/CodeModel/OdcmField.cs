// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Vipr.Core.CodeModel
{
    public class OdcmField : OdcmObject
    {
        public OdcmClass Class { get; set; }

        public bool ContainsTarget { get; set; }

        public bool IsLink { get; set; }

        public bool IsRequired { get; set; }

        public OdcmType Type { get; set; }

        public bool IsCollection { get; set; }

        public OdcmField(string name)
            : base(name)
        {
        }
    }
}
