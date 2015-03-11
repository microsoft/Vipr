// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Dynamic;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class FetcherExpandMethod : Method
    {
        public OdcmClass OdcmClass { get; private set; }

        private FetcherExpandMethod(OdcmClass odcmClass)
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
            ReturnType = new Type(NamesService.GetFetcherInterfaceName(odcmClass));
            OdcmClass = odcmClass;
        }

        public static FetcherExpandMethod ForFetcher(OdcmClass odcmClass)
        {
            return new FetcherExpandMethod(odcmClass);
        }

        public static FetcherExpandMethod ForConcrete(OdcmClass odcmClass)
        {
            return new FetcherExpandMethod(odcmClass)
            {
                DefiningInterface = NamesService.GetFetcherInterfaceName(odcmClass)
            };
        }
    }
}