// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core;
using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp.Lite
{
    public class CollectionGetByIdIndexer : Indexer
    {
        public Dictionary<Parameter, OdcmProperty> ParameterToPropertyMap { get; private set; }

        public CollectionGetByIdIndexer(OdcmEntityClass odcmClass, OdcmProjection projection)
        {
            ParameterToPropertyMap = odcmClass.Key.ToDictionary(Parameter.FromProperty, p => p);

            Parameters = global::Vipr.Writer.CSharp.Lite.Parameters.GetKeyParameters(odcmClass);
            ReturnType = new Type(NamesService.GetFetcherInterfaceName(odcmClass, projection));
            OdcmClass = odcmClass;

            IsSettable = false;
            IsGettable = true;
        }

        public static CollectionGetByIdIndexer ForCollectionClass(OdcmEntityClass odcmClass, OdcmProjection projection)
        {
            return new CollectionGetByIdIndexer(odcmClass, projection)
            {
                DefiningInterface = NamesService.GetCollectionInterfaceName(odcmClass, projection)
            };
        }

        public OdcmClass OdcmClass { get; private set; }
    }
}
