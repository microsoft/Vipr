// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Dynamic;
using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp.Lite
{
    public class FetcherExpandMethod : Method
    {
        public OdcmClass OdcmClass { get; private set; }

        private FetcherExpandMethod(OdcmClass odcmClass, OdcmProjection projection = null)
        {
            Visibility = Visibility.Public;
            Name = "Expand";
            GenericParameters = new[] { "TTarget" };
            Parameters = new[]
            {
                new Parameter(new Type(new Identifier("System.Linq.Expressions", "Expression"),
                    new Type(new Identifier("System", "Func"), new Type(NamesService.GetConcreteInterfaceName(odcmClass)),
                        new Type(new Identifier(null, "TTarget")))), "navigationPropertyAccessor"),
            };
            ReturnType = new Type(NamesService.GetFetcherInterfaceName(odcmClass, projection));
            OdcmClass = odcmClass;
        }

        public static FetcherExpandMethod ForFetcherInterface(OdcmClass odcmClass, OdcmProjection projection)
        {
            return new FetcherExpandMethod(odcmClass, projection);
        }

        public static FetcherExpandMethod ForFetcherClass(OdcmClass odcmClass, OdcmProjection projection)
        {
            return new FetcherExpandMethod(odcmClass, projection)
            {
                DefiningInterface = NamesService.GetFetcherInterfaceName(odcmClass, projection)
            };
        }
    }
}