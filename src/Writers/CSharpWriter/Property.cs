// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class Property : InterfaceProperty
    {
        public string FieldName { get; protected set; }

        protected Property(OdcmProperty property)
            : this(property.Name)
        {
        }

        protected Property(string name)
        {
            Name = NamesService.GetPropertyName(name);
        }

        public static NavigationProperty AsConcreteNavigationProperty(OdcmProperty odcmProperty)
        {
            return odcmProperty.IsCollection
                ? ConcreteNavigationCollectionProperty.ForConcrete(odcmProperty)
                : ConcreteNavigationProperty.ForConcrete(odcmProperty);
        }

        public static ConcreteNavigationAccessorProperty AsNavigationAccessorProperty(OdcmProperty odcmProperty)
        {
            return odcmProperty.IsCollection
                ? ConcreteNavigationCollectionAccessorProperty.ForConcrete(odcmProperty)
                : ConcreteNavigationAccessorProperty.ForConcrete(odcmProperty);
        }

        public static StructuralProperty AsConcreteStructuralProperty(OdcmProperty odcmProperty)
        {
            return odcmProperty.IsCollection
                ? StructuralCollectionProperty.ForConcrete(odcmProperty)
                : StructuralProperty.ForConcrete(odcmProperty);
        }

        public static FetcherNavigationProperty AsIFetcherNavigationPropertyForConcrete(OdcmProperty odcmProperty)
        {
            return odcmProperty.IsCollection
                ? FetcherNavigationCollectionProperty.ForConcrete(odcmProperty)
                : FetcherNavigationProperty.ForConcrete(odcmProperty);
        }

        public static FetcherNavigationProperty AsIFetcherNavigationPropertyForFetcher(OdcmProperty odcmProperty)
        {
            return odcmProperty.IsCollection
                ? FetcherNavigationCollectionProperty.ForFetcher(odcmProperty)
                : FetcherNavigationProperty.ForFetcher(odcmProperty);
        }

        public static NavigationProperty AsContainerNavigationProperty(OdcmProperty odcmProperty)
        {
            return odcmProperty.IsCollection
                ? (NavigationProperty)ContainerNavigationCollectionProperty.ForService(odcmProperty)
                : ContainerNavigationProperty.ForService(odcmProperty);
        }

        public static Property AsObsoletedProperty(OdcmProperty odcmProperty)
        {
            return new ObsoletedProperty(odcmProperty);
        }

        public static Property AsObsoletedNavigationProperty(OdcmProperty odcmProperty)
        {
            return new ObsoletedNavigationProperty(odcmProperty);
        }
    }
}
