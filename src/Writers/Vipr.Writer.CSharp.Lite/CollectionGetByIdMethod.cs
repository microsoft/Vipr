// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp.Lite
{
    public class CollectionGetByIdMethod : Method
    {
        public OdcmClass OdcmClass { get; private set; }

        public Dictionary<Parameter, OdcmProperty> ParameterToPropertyMap { get; private set; }

        public CollectionGetByIdMethod(OdcmClass odcmClass, OdcmProjection projection = null)
        {
            Name = "GetById";
            ParameterToPropertyMap = ((OdcmEntityClass)odcmClass).Key.ToDictionary(Parameter.FromProperty, p => p);
            Parameters = global::Vipr.Writer.CSharp.Lite.Parameters.GetKeyParameters(odcmClass);
            ReturnType = new Type(NamesService.GetFetcherInterfaceName(odcmClass, projection));
            OdcmClass = odcmClass;
        }

        public static CollectionGetByIdMethod ForCollectionClass(OdcmClass odcmClass, OdcmProjection projection = null)
        {
            return new CollectionGetByIdMethod(odcmClass, projection)
            {
                DefiningInterface = NamesService.GetCollectionInterfaceName(odcmClass, projection)
            };
        }
    }
}
