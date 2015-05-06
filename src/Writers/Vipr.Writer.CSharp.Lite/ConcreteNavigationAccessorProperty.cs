// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp.Lite
{
    public class ConcreteNavigationAccessorProperty : NavigationProperty
    {
        public Identifier InstanceType { get; internal set; }
        public Identifier FieldType { get; internal set; }

        protected ConcreteNavigationAccessorProperty(OdcmProperty odcmProperty) : base(odcmProperty)
        {
            FieldName = NamesService.GetPropertyFieldName(odcmProperty);
            InstanceType = NamesService.GetFetcherTypeName(odcmProperty.Type);
            FieldType = NamesService.GetConcreteTypeName(odcmProperty.Type);
            Type = new Type(NamesService.GetConcreteTypeName(odcmProperty.Type));
        }

        public static ConcreteNavigationAccessorProperty ForConcrete(OdcmProperty odcmProperty)
        {
            return new ConcreteNavigationAccessorProperty(odcmProperty);
        }
    }
}
