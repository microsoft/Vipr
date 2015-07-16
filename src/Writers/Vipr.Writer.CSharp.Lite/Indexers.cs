// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp.Lite
{
    public class Indexers
    {
        public static IEnumerable<Indexer> ForCollection(OdcmEntityClass odcmClass)
        {
            var retVal = new List<Indexer>();
            foreach (var projection in odcmClass.Projections)
            {
                retVal.Add(CollectionGetByIdIndexer.ForCollectionClass(odcmClass, projection));
            }

            return retVal;
        }

        public static IEnumerable<Indexer> Empty { get { return Enumerable.Empty<Indexer>(); } }
    }
}