// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public abstract class IndexerSignature : ParameterizedFunction
    {
        public bool IsSettable { get; protected set; }
        public bool IsGettable { get; protected set; }

        public static IEnumerable<IndexerSignature> ForCollectionInterface(OdcmEntityClass odcmClass)
        {
            return new IndexerSignature[]
            {
                new CollectionGetByIdIndexer(odcmClass)
            };
        }
    }
}
