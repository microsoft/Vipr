// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class Constructors
    {
        public static IEnumerable<Constructor> ForComplex(OdcmClass odcmClass)
        {
            var classIdentifier = NamesService.GetConcreteTypeName(odcmClass);

            return DefaultOnly(odcmClass, classIdentifier);
        }

        public static IEnumerable<Constructor> ForConcrete(OdcmClass odcmClass)
        {
            var classIdentifier = NamesService.GetConcreteTypeName(odcmClass);

            return DefaultOnly(odcmClass, classIdentifier);
        }

        public static IEnumerable<Constructor> ForFetcher(OdcmClass odcmClass)
        {
            var classIdentifier = NamesService.GetFetcherTypeName(odcmClass);

            return DefaultOnly(odcmClass, classIdentifier);
        }

        public static IEnumerable<Constructor> ForCollection(OdcmClass odcmClass)
        {
            return new[] { new CollectionConstructor(odcmClass) };
        }

        private static IEnumerable<Constructor> DefaultOnly(OdcmClass odcmClass, Identifier classIdentifier)
        {
            return new[] { new DefaultConstructor(odcmClass, classIdentifier) };
        }

        public static IEnumerable<Constructor> ForEntityContainer(OdcmClass odcmContainer)
        {
            return new[] { new EntityContainerConstructor(odcmContainer) };
        }

        public static IEnumerable<Constructor> Empty { get { return Enumerable.Empty<Constructor>(); } }
    }
}