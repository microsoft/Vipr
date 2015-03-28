// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Vipr.Core.CodeModel
{
    public class OdcmProperty : OdcmObject
    {
        public OdcmClass Class { get; set; }

        public bool ReadOnly { get; set; }

        [Obsolete("This property will be retired in the future. Use 'Projection' property instead")]
        public OdcmType Type
        {
            get
            {
                if (Projection != null)
                    return Projection.Type;

                return null;
            }
        }

        public OdcmProjection Projection { get; set; }

        public bool ContainsTarget { get; set; }

        public bool IsLink { get; set; }

        public bool IsRequired { get; set; }

        public bool IsCollection { get; set; }

        public bool IsNullable { get; set; }

        public string DefaultValue { get; set; }

        public OdcmProperty(string name)
            : base(name)
        {
        }
    }
}
