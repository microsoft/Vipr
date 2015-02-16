// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class StructuralProperty : Property
    {
        public string ModelName { get; private set; }

        protected StructuralProperty(OdcmProperty odcmProperty) : base(odcmProperty)
        {
            FieldName = NamesService.GetPropertyFieldName(odcmProperty);
            ModelName = NamesService.GetModelPropertyName(odcmProperty);
            Type = TypeService.GetPropertyType(odcmProperty);
        }

        public static StructuralProperty ForConcrete(OdcmProperty odcmProperty)
        {
            return new StructuralProperty(odcmProperty);
        }
    }
}
