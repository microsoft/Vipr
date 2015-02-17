// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class EnumMember
    {
        public string Name { get; set; }
        public long? Value { get; set; }
        
        public EnumMember(OdcmEnumMember odcmEnumMember)
        {
            Name = odcmEnumMember.Name;
            Value = odcmEnumMember.Value;
        }
    }
}