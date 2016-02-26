// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Dynamic;
using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp.Lite
{
    public class FetcherDeleteLinkMethod : Method
    {
        public OdcmClass OdcmClass { get; private set; }

        private FetcherDeleteLinkMethod(OdcmClass odcmClass)
        {
            Visibility = Visibility.Public;
            IsOverriding = odcmClass.Base is OdcmClass && !((OdcmClass)odcmClass.Base).IsAbstract;
            Name = "DeleteLinkAsync";
            Parameters = new[]
            {
                new Parameter(new Type(new Identifier("System", "Object")), "source"),
                new Parameter(new Type(NamesService.GetPrimitiveTypeName("Boolean")), "deferSaveChanges", "false")
            };
            ReturnType = new Type(Identifier.Task);
            OdcmClass = odcmClass;
        }

        public static FetcherDeleteLinkMethod ForFetcher(OdcmClass odcmClass)
        {
            return new FetcherDeleteLinkMethod(odcmClass);
        }
    }
}