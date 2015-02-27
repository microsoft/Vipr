// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class Indexers
    {
        public static IEnumerable<Indexer> ForCollection(OdcmEntityClass odcmClass)
        {
            return new Indexer[]
            {
                new CollectionGetByIdIndexer(odcmClass)
            };
        }

        public static IEnumerable<Indexer> Empty { get { return Enumerable.Empty<Indexer>(); } }
    }
}