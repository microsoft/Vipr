// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class ConcreteNavigationAccessorProperty : NavigationProperty
    {
        public Identifier InstanceType { get; internal set; }

        protected ConcreteNavigationAccessorProperty(OdcmProperty odcmProperty) : base(odcmProperty)
        {
            FieldName = NamesService.GetPropertyFieldName(odcmProperty);
            InstanceType = NamesService.GetFetcherTypeName(odcmProperty.Projection.Type);
            Type = new Type(NamesService.GetConcreteTypeName(odcmProperty.Projection.Type));
        }

        public static ConcreteNavigationAccessorProperty ForConcrete(OdcmProperty odcmProperty)
        {
            return new ConcreteNavigationAccessorProperty(odcmProperty);
        }
    }
}
