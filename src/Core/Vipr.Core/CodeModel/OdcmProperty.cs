// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Vipr.Core.CodeModel
{
    public class OdcmProperty : OdcmObject
    {
        public OdcmClass Class { get; set; }
        public OdcmProperty ParentPropertyType { get; set; }
        public List<OdcmProperty> ChildPropertyTypes { get; set; } = new List<OdcmProperty>();

        public bool ReadOnly { get; set; }

//        [Obsolete("This property will be retired in the future. Use 'Projection' property instead")]
        public OdcmType Type
        {
            get
            {
                if (Projection != null)
                    return Projection.Type;

                return null;
            }
        }

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

        public OdcmProperty Clone(string name)
        {
            return new OdcmProperty(name ?? Name?.Clone() as string)
            {
                ContainsTarget = ContainsTarget,
                IsLink = IsLink,
                IsRequired = IsRequired,
                IsCollection = IsCollection,
                IsNullable = IsNullable,
                DefaultValue = DefaultValue?.Clone() as string,
                Class = Class,
                Description = Description?.Clone() as string,
                LongDescription = LongDescription?.Clone() as string,
                ParentPropertyType = ParentPropertyType,
                Projection = Projection?.Clone() as OdcmProjection,
                ReadOnly = ReadOnly,
            };
        }
    }
}
