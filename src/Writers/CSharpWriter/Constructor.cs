// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public abstract class Constructor : ParameterizedFunction
    {
        public string Name { get; protected set; }

        public static IEnumerable<Constructor> ForCollectionInterface(OdcmClass odcmClass)
        {
            return new Constructor[]
            {
                new CollectionConstructor(odcmClass)
            }.AsEnumerable();
        }
    }
}
