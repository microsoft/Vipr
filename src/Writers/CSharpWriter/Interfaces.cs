// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class Interfaces
    {
        internal static IEnumerable<Interface> Empty { get { return Enumerable.Empty<Interface>(); } } 

        internal static IEnumerable<Interface> ForOdcmClassEntity(OdcmEntityClass odcmClass)
        {
            return new[]
            {
                Interface.ForConcrete(odcmClass),
                Interface.ForFetcher(odcmClass),
                Interface.ForCollection(odcmClass),
            };
        }

        internal static IEnumerable<Interface> ForOdcmClassService(OdcmClass odcmClass)
        {
            return new[] {Interface.ForEntityContainer(odcmClass)};
        }

        internal static IEnumerable<Interface> ForUpcastMethods(OdcmEntityClass odcmClass)
        {
            return new[] {Interface.ForFetcherUpcastMethods(odcmClass)};
        }
    }
}