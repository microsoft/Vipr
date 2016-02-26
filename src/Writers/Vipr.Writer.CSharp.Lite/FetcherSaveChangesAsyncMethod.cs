// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Dynamic;
using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp.Lite
{
    public class FetcherSaveChangesAsyncMethod : Method
    {
        public OdcmClass OdcmClass { get; private set; }

        private FetcherSaveChangesAsyncMethod(OdcmClass odcmClass)
        {
            Visibility = Visibility.Public;
            IsOverriding = odcmClass.Base is OdcmClass && !((OdcmClass)odcmClass.Base).IsAbstract;
            Name = "SaveChangesAsync";
            Parameters = new[]
            {
                new Parameter(new Type(NamesService.GetPrimitiveTypeName("Boolean")), "deferSaveChanges", "false"),
                new Parameter(new Type(new Identifier("Microsoft.OData.Client", "SaveChangesOptions")), "saveChangesOption", "SaveChangesOptions.None")
            };
            ReturnType = new Type(Identifier.Task);
            OdcmClass = odcmClass;
        }

        public static FetcherSaveChangesAsyncMethod ForFetcher(OdcmClass odcmClass)
        {
            return new FetcherSaveChangesAsyncMethod(odcmClass);
        }
    }
}