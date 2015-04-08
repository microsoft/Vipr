// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp
{
    internal class ConcreteNavigationCollectionAccessorProperty : ConcreteNavigationAccessorProperty
    {
        public Type CollectionType { get; private set; }

        private ConcreteNavigationCollectionAccessorProperty(OdcmProperty odcmProperty) : base(odcmProperty)
        {
            CollectionType = new Type(NamesService.GetCollectionTypeName((OdcmClass)odcmProperty.Type));
            FieldName = NamesService.GetConcreteFieldName(odcmProperty);
            InstanceType = NamesService.GetConcreteTypeName(odcmProperty.Type);
            Type = new Type(new Identifier("global::System.Collections.Generic", "IList"),
                new Type(NamesService.GetConcreteTypeName(odcmProperty.Type)));
        }

        public new static ConcreteNavigationCollectionAccessorProperty ForConcrete(OdcmProperty odcmProperty)
        {
            return new ConcreteNavigationCollectionAccessorProperty(odcmProperty);
        }
    }
}