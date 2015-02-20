// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class Properties
    {
        public static IEnumerable<Property> ForComplex(OdcmClass odcmClass)
        {
            return GetStructuralProperties(odcmClass)
                .Concat(GetObsoletedStructuralProperties(odcmClass));
        }

        public static IEnumerable<Property> ForConcreteInterface(OdcmClass odcmClass)
        {
            return GetStructuralProperties(odcmClass)
                .Concat(GetIConcreteNavigationPropertiesForConcrete(odcmClass));
        }

        public static IEnumerable<Property> ForConcrete(OdcmClass odcmClass)
        {
            return ForConcreteInterface(odcmClass)
                .Concat(GetObsoletedStructuralProperties(odcmClass))
                .Concat(GetObsoletedNavigationProperties(odcmClass))
                .Concat(GetImplicitPropertiesForConcrete(odcmClass))
                .Concat(GetIFetcherPropertiesForConcrete(odcmClass));
        }

        public static IEnumerable<Property> ForFetcherInterface(OdcmClass odcmClass)
        {
            return GetIFetcherNavigationPropertiesForFetcher(odcmClass);
        }

        public static IEnumerable<Property> ForFetcher(OdcmClass odcmClass)
        {
            return ForFetcherInterface(odcmClass);
        }

        public static IEnumerable<Property> ForEntityContainerInterface(OdcmClass odcmContainer)
        {
            return GetNavigationPropertiesForContainer(odcmContainer);
        }

        public static IEnumerable<Property> ForEntityContainer(OdcmClass odcmContainer)
        {
            return Properties.ForEntityContainerInterface(odcmContainer)
                .Concat(new Property[]
                {
                    new AutoProperty("Context", new Type(NamesService.GetExtensionTypeName("DataServiceContextWrapper")),
                        privateSet: true)
                });
        }

        private static IEnumerable<Property> GetImplicitPropertiesForConcrete(OdcmClass odcmClass)
        {
            return odcmClass.NavigationProperties().Select(Property.AsNavigationAccessorProperty);
        }

        private static IEnumerable<Property> GetIConcreteNavigationPropertiesForConcrete(OdcmClass odcmClass)
        {
            return odcmClass.NavigationProperties().Select(Property.AsConcreteNavigationProperty);
        }

        private static IEnumerable<Property> GetIFetcherPropertiesForConcrete(OdcmClass odcmClass)
        {
            return odcmClass.NavigationProperties().Select(Property.AsIFetcherNavigationPropertyForConcrete);
        }

        private static IEnumerable<FetcherNavigationProperty> GetIFetcherNavigationPropertiesForFetcher(OdcmClass odcmClass)
        {
            return odcmClass.NavigationProperties().Select(Property.AsIFetcherNavigationPropertyForFetcher);
        }

        private static IEnumerable<NavigationProperty> GetNavigationPropertiesForContainer(OdcmClass odcmClass)
        {
            return odcmClass.NavigationProperties().Select(Property.AsContainerNavigationProperty);
        }

        private static IEnumerable<Property> GetObsoletedStructuralProperties(OdcmClass odcmClass)
        {
            return odcmClass.StructuralProperties().Where(p => NamesService.GetPropertyName(p) != NamesService.GetModelPropertyName(p))
                    .Select(Property.AsObsoletedProperty);
        }

        private static IEnumerable<Property> GetObsoletedNavigationProperties(OdcmClass odcmClass)
        {
            return odcmClass.NavigationProperties().Where(p => NamesService.GetPropertyName(p) != NamesService.GetModelPropertyName(p))
                    .Select(Property.AsObsoletedNavigationProperty);
        }

        private static IEnumerable<Property> GetStructuralProperties(OdcmClass odcmClass)
        {
            return odcmClass.StructuralProperties().Select(Property.AsConcreteStructuralProperty);
        }

        public static IEnumerable<Property> Empty { get { return Enumerable.Empty<Property>(); }}
    }
}