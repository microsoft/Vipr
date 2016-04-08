// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core;
using Vipr.Core.CodeModel;
using Vipr.Core.CodeModel.Vocabularies.Capabilities;

namespace Vipr.Writer.CSharp.Lite
{
    public class Methods
    {
        public static IEnumerable<Method> ForConcreteMediaInterface(OdcmClass odcmClass)
        {
            return Methods.ForEntityType(odcmClass);
        }

        public static IEnumerable<Method> ForFetcherInterfaceUpcasts(OdcmClass odcmClass)
        {
            return odcmClass.NestedDerivedTypes()
                    .Select(dr => new FetcherUpcastMethod(odcmClass, dr));
        }

        public static IEnumerable<Method> ForFetcherUpcasts(OdcmClass odcmClass)
        {
            return Methods.ForFetcherInterfaceUpcasts(odcmClass);
        }

        public static IEnumerable<Method> ForFetcherInterface(OdcmClass odcmClass, OdcmProjection projection)
        {
            var retVal = new List<Method>();

            retVal.AddRange(Methods.ForEntityType(odcmClass));

            if (!odcmClass.IsAbstract)
            {
                retVal.Add(new FetcherExecuteAsyncMethod(odcmClass));
                if (projection.SupportsExpand())
                {
                    retVal.Add(FetcherExpandMethod.ForFetcherInterface(odcmClass, projection));
                }

                if (projection.SupportsUpdate())
                {
                    retVal.Add(FetcherUpdateMethod.ForFetcher(odcmClass));
                    retVal.Add(FetcherSetMethod.ForFetcher(odcmClass));
                }

                if (projection.SupportsUpdateLink())
                {
                    retVal.Add(FetcherUpdateLinkMethod.ForFetcher(odcmClass));
                }

                if (projection.SupportsDelete())
                {
                    retVal.Add(FetcherDeleteMethod.ForFetcher(odcmClass));
                }

                if (projection.SupportsDeleteLink())
                {
                    retVal.Add(FetcherDeleteLinkMethod.ForFetcher(odcmClass));
                }

                retVal.Add(FetcherSaveChangesAsyncMethod.ForFetcher(odcmClass));
            }

            return retVal;
        }

        public static IEnumerable<Method> ForFetcherClass(OdcmClass odcmClass)
        {
            var retVal = new List<Method>();

            retVal.AddRange(Methods.ForEntityType(odcmClass));

            if (!odcmClass.IsAbstract)
            {
                retVal.Add(new FetcherExecuteAsyncMethod(odcmClass));

                foreach (var projection in odcmClass.DistinctProjections())
                {
                    if (projection.SupportsExpand())
                    {
                        retVal.Add(FetcherExpandMethod.ForFetcherClass(odcmClass, projection));
                    }
                }

                retVal.Add(FetcherUpdateMethod.ForFetcher(odcmClass));
                retVal.Add(FetcherSetMethod.ForFetcher(odcmClass));
                retVal.Add(FetcherUpdateLinkMethod.ForFetcher(odcmClass));
                retVal.Add(FetcherDeleteMethod.ForFetcher(odcmClass));
                retVal.Add(FetcherDeleteLinkMethod.ForFetcher(odcmClass));
                retVal.Add(FetcherSaveChangesAsyncMethod.ForFetcher(odcmClass));
            }

            if (!odcmClass.IsAbstract)
            {
                retVal.Add(new EnsureQueryMethod(odcmClass));
            }

            return retVal;
        }

        public static IEnumerable<Method> ForCollectionInterface(OdcmClass odcmClass, OdcmProjection projection)
        {
            var odcmMediaClass = odcmClass as OdcmMediaClass;
            if (odcmMediaClass != null)
            {
                return ForMediaCollectionInterface(odcmMediaClass);
            }

            var retVal = new List<Method>();
            retVal.AddRange(Methods.GetMethodsBoundToCollection(odcmClass));


            retVal.Add(new CollectionGetByIdMethod(odcmClass, projection));
            retVal.Add(new CollectionExecuteAsyncMethod(odcmClass));

            if (projection.SupportsUpdateLink())
            {
                retVal.Add(new CollectionAddLinkAsyncMethod(odcmClass));
            }
            if (projection.SupportsDeleteLink())
            {
                retVal.Add(new CollectionRemoveLinkAsyncMethod(odcmClass));
            }
            if (projection.SupportsInsert())
            {
                retVal.Add(new CollectionAddAsyncMethod(odcmClass));
            }
            if (projection.SupportsUpdate())
            {
                retVal.Add(new CollectionUpdateAsyncMethod(odcmClass));
            }
            if (projection.SupportsDelete())
            {
                retVal.Add(new CollectionDeleteAsyncMethod(odcmClass));
            }

            return retVal;
        }

        public static IEnumerable<Method> ForMediaCollectionInterface(OdcmMediaClass odcmClass)
        {
            return Methods.GetMethodsBoundToCollection(odcmClass)
                .Concat(new Method[]
                {
                    new CollectionGetByIdMethod(odcmClass),
                    new CollectionExecuteAsyncMethod(odcmClass),
                    new AddAsyncMediaMethod(odcmClass)
                });
        }

        public static IEnumerable<Method> ForCollectionClass(OdcmClass odcmClass)
        {
            var odcmMediaClass = odcmClass as OdcmMediaClass;
            if (odcmMediaClass != null)
            {
                return ForMediaCollectionInterface(odcmMediaClass);
            }

            var retVal = new List<Method>();
            retVal.AddRange(Methods.GetMethodsBoundToCollection(odcmClass));

            foreach (var projection in odcmClass.DistinctProjections())
            {
                retVal.Add(CollectionGetByIdMethod.ForCollectionClass(odcmClass, projection));
            }

            retVal.Add(new CollectionExecuteAsyncMethod(odcmClass));
            retVal.Add(new CollectionAddLinkAsyncMethod(odcmClass));
            retVal.Add(new CollectionRemoveLinkAsyncMethod(odcmClass));
            retVal.Add(new CollectionAddAsyncMethod(odcmClass));
            retVal.Add(new CollectionUpdateAsyncMethod(odcmClass));
            retVal.Add(new CollectionDeleteAsyncMethod(odcmClass));

            return retVal;
        }

        public static IEnumerable<Method> ForEntityContainerInterface(OdcmClass odcmContainer)
        {
            return Methods.ForEntityType(odcmContainer);
        }

        public static IEnumerable<Method> ForEntityContainer(OdcmServiceClass odcmContainer)
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

        public static IEnumerable<Method> Empty { get { return Enumerable.Empty<Method>(); } }

        private static IEnumerable<Method> ForEntityType(OdcmClass odcmClass)
        {
            return GetMethodsBoundToEntityType(odcmClass);
        }

        private static IEnumerable<Method> ForContainerAddToCollection(OdcmClass odcmClass)
        {
            return odcmClass.NavigationProperties()
                    .Where(p => p.IsCollection)
                    .Select(p => new ContainerAddToCollectionMethod(p));
        }

        internal static IEnumerable<Method> ForConcreteUpcasts(OdcmClass odcmClass)
        {
            return odcmClass.NestedDerivedTypes()
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

        public static IEnumerable<Method> ForCountableCollectionInterface(OdcmClass odcmClass)
        {
            return new[] { new CollectionCountAsyncMethod() };
        }
    }
}