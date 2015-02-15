// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class Methods
    {
        public static IEnumerable<Method> ForConcreteInterface(OdcmClass odcmClass)
        {
            return Methods.ForEntityType(odcmClass);
        }

        public static IEnumerable<Method> ForConcrete(OdcmClass odcmClass)
        {
            var retVal = Methods.ForConcreteInterface(odcmClass).ToList();

            retVal.Add(new EnsureQueryMethod(odcmClass));

            retVal.AddRange(Methods.ForConcreteUpcasts(odcmClass));

            if (!odcmClass.IsAbstract)
            {
                retVal.Add(new ConcreteExecuteAsyncMethod(odcmClass));
                retVal.Add(FetcherExpandMethod.ForConcrete(odcmClass));
            }

            return retVal;
        }

        public static IEnumerable<Method> ForFetcherInterface(OdcmClass odcmClass)
        {
            var retVal = new List<Method>();

            retVal.AddRange(Methods.ForEntityType(odcmClass));

            retVal.AddRange(Methods.ForFetcherUpcasts(odcmClass));

            if (!odcmClass.IsAbstract)
            {
                retVal.Add(new FetcherExecuteAsyncMethod(odcmClass));
                retVal.Add(FetcherExpandMethod.ForFetcher(odcmClass));
            }

            return retVal;
        }

        public static IEnumerable<Method> ForFetcher(OdcmClass odcmClass)
        {
            var retVal = Methods.ForFetcherInterface(odcmClass).ToList();

            if (!odcmClass.IsAbstract)
            {
                retVal.Add(new EnsureQueryMethod(odcmClass));
            }

            return retVal;
        }

        public static IEnumerable<Method> ForCollectionInterface(OdcmClass odcmClass)
        {
            return Methods.GetMethodsBoundToCollection(odcmClass)
                .Concat(new Method[]
                {
                    new CollectionGetByIdMethod(odcmClass),
                    new CollectionExecuteAsyncMethod(odcmClass),
                    new AddAsyncMethod(odcmClass)
                });
        }

        public static IEnumerable<Method> ForCollection(OdcmClass odcmClass)
        {
            return Methods.ForCollectionInterface(odcmClass);
        }

        public static IEnumerable<Method> ForEntityContainerInterface(OdcmClass odcmContainer)
        {
            return Methods.ForEntityType(odcmContainer);
        }

        public static IEnumerable<Method> ForEntityContainer(OdcmClass odcmContainer)
        {
            return Methods.ForEntityContainerInterface(odcmContainer)
                .Concat(Methods.ForContainerAddToCollection(odcmContainer))
                .Concat(new Method[]
                {
                    new ContainerTypeFromNameMethod(odcmContainer),
                    new ContainerNameFromTypeMethod(odcmContainer),
                    new ContainerGetPathMethod(),
                    new ContainerGetUrlMethod()
                });
        }

        public static IEnumerable<Method> ForGeneratedEdmModel()
        {
            return new Method[]
            {
                new GeneratedEdmModelGetInstanceMethod(),
                new GeneratedEdmModelLoadModelFromStringMethod(),
                new GeneratedEdmModelCreateXmlReaderMethod(),
            };
        }

        public static IEnumerable<Method> Emtpy { get { return Enumerable.Empty<Method>(); } }

        private static IEnumerable<Method> ForEntityType(OdcmClass odcmClass)
        {
            return GetMethodsBoundToEntityType(odcmClass);
        }

        private static IEnumerable<Method> ForFetcherUpcasts(OdcmClass odcmClass)
        {
            return ConfigurationService.Settings.OmitFetcherUpcastMethods
                ? Methods.Emtpy
                : odcmClass.NestedDerivedTypes()
                    .Select(dr => new FetcherUpcastMethod(odcmClass, dr));
        }

        private static IEnumerable<Method> ForContainerAddToCollection(OdcmClass odcmClass)
        {
            return odcmClass.NavigationProperties()
                    .Where(p => p.IsCollection)
                    .Select(p => new ContainerAddToCollectionMethod(p));
        }

        private static IEnumerable<Method> ForConcreteUpcasts(OdcmClass odcmClass)
        {
            return ConfigurationService.Settings.OmitFetcherUpcastMethods
                ? Methods.Emtpy
                : odcmClass.NestedDerivedTypes()
                    .Select(dr => new ConcreteUpcastMethod(odcmClass, dr));
        }

        private static IEnumerable<Method> GetMethodsBoundToCollection(OdcmClass odcmClass)
        {
            return odcmClass.Methods.Where(m => m.IsBoundToCollection).Select(Method.ForEntityType);
        }

        private static IEnumerable<Method> GetMethodsBoundToEntityType(OdcmClass odcmClass)
        {
            return odcmClass.Methods.Where(m => !m.IsBoundToCollection).Select(Method.ForEntityType);
        }
    }
}