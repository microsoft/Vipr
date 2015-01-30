// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class StructuralProperty : Property
    {
        protected StructuralProperty(OdcmProperty odcmProperty) : base(odcmProperty)
        {
            FieldName = NamesService.GetPropertyFieldName(odcmProperty);
            Type = TypeService.GetPropertyType(odcmProperty);
        }

        public static StructuralProperty ForConcrete(OdcmProperty odcmProperty)
        {
            return new StructuralProperty(odcmProperty);
        }
    }
}
