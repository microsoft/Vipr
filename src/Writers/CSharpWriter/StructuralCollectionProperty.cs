// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class StructuralCollectionProperty : StructuralProperty
    {
        public Type InstanceType { get; private set; }

        private StructuralCollectionProperty(OdcmProperty odcmProperty) : base(odcmProperty)
        {
            InstanceType = new Type(NamesService.GetPublicTypeName(odcmProperty.Type));
            Type = new Type(new Identifier("System.Collections.Generic", "IList"), InstanceType);
        }

        public new static StructuralProperty ForConcrete(OdcmProperty odcmProperty)
        {
            return new StructuralCollectionProperty(odcmProperty);
        }
    }
}