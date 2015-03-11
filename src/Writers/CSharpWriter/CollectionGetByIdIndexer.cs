// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class CollectionGetByIdIndexer : Indexer
    {
        public Dictionary<Parameter, OdcmProperty> ParameterToPropertyMap { get; private set; }

        public CollectionGetByIdIndexer(OdcmEntityClass odcmClass)
        {
            ParameterToPropertyMap = odcmClass.Key.ToDictionary(Parameter.FromProperty, p => p);

            Parameters = global::CSharpWriter.Parameters.GetKeyParameters(odcmClass);
            ReturnType = new Type(NamesService.GetFetcherInterfaceName(odcmClass));
            OdcmClass = odcmClass;

            IsSettable = false;
            IsGettable = true;
        }

        public OdcmClass OdcmClass { get; private set; }
    }
}
