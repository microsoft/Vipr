// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Dynamic;
using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp.Lite
{
    public class FetcherSetMethod : Method
    {
        public OdcmClass OdcmClass { get; private set; }

        private FetcherSetMethod(OdcmClass odcmClass)
        {
            Visibility = Visibility.Public;
            Name = "SetAsync";
            Parameters = new[]
            {
                new Parameter(new Type(new Identifier("System", "Object")), "source"),
                new Parameter(new Type(NamesService.GetConcreteInterfaceName(odcmClass)), "target"),
                new Parameter(new Type(NamesService.GetPrimitiveTypeName("Boolean")), "deferSaveChanges", "false")
            };
            ReturnType = new Type(Identifier.Task);
            OdcmClass = odcmClass;
        }

        public static FetcherSetMethod ForFetcher(OdcmClass odcmClass)
        {
            return new FetcherSetMethod(odcmClass);
        }
    }
}