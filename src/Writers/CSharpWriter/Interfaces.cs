// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class Interfaces
    {
        public static IEnumerable<Type> ForFetcher(OdcmClass odcmClass)
        {
            return new List<Type> { new Type(NamesService.GetFetcherInterfaceName(odcmClass)) };
        }

        public static IEnumerable<Type> ForConcrete(OdcmClass odcmClass)
        {
            var retVal = new List<Type>
            {
                new Type(NamesService.GetConcreteInterfaceName(odcmClass)),
                new Type(NamesService.GetFetcherInterfaceName(odcmClass))
            };

            return retVal;
        }

        public static IEnumerable<Type> ForCollection(OdcmClass odcmClass)
        {
            return new List<Type> { new Type(NamesService.GetCollectionInterfaceName(odcmClass)) };
        }

        public static IEnumerable<Type> ForEntityContainer(OdcmClass odcmContainer)
        {
            return new List<Type> { new Type(NamesService.GetEntityContainerInterfaceName(odcmContainer)) };
        }

        public static IEnumerable<Type> ForConcreteInterface(OdcmClass odcmClass)
        {
            var retVal = new List<Type>();

            var baseClass = odcmClass.Base as OdcmClass;

            var interfaceIdentifier = baseClass == null ?
                NamesService.GetExtensionTypeName("IEntityBase") :
                NamesService.GetConcreteInterfaceName(baseClass);

            retVal.Add(new Type(interfaceIdentifier));

            return retVal;
        }

        public static IEnumerable<Type> ForFetcherInterface(OdcmClass odcmClass)
        {
            var retVal = new List<Type>();

            var baseClass = odcmClass.Base as OdcmClass;

            if (baseClass != null)
            {
                retVal.Add(new Type(NamesService.GetFetcherInterfaceName(baseClass)));
            }

            return retVal;
        }

        public static IEnumerable<Type> Empty { get { return Enumerable.Empty<Type>(); } }
    }
}