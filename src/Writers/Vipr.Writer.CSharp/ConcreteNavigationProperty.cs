// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp
{
    public class ConcreteNavigationProperty : NavigationProperty
    {
        protected ConcreteNavigationProperty(OdcmProperty odcmProperty) : base(odcmProperty)
        {
            FieldName = NamesService.GetPropertyFieldName(odcmProperty);
            FieldType = NamesService.GetConcreteTypeName(odcmProperty.Type);
            Type = new Type(NamesService.GetConcreteInterfaceName(odcmProperty.Type));
        }

        public Identifier FieldType { get; set; }

        public static ConcreteNavigationProperty ForConcrete(OdcmProperty odcmProperty)
        {
            return new ConcreteNavigationProperty(odcmProperty)
            {
                DefiningInterface = NamesService.GetConcreteInterfaceName(odcmProperty.Class),
            };
        }
    }
}
