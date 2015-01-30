// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Vipr.Core.CodeModel
{
    public class OdcmEnumMember : OdcmObject
    {
        public long? Value { get; set; }

        public OdcmEnumMember(string name)
            : base(name)
        {
        }
    }
}
