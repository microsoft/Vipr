// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp.Lite
{
    public class FetcherUpcastMethod : UpcastMethodBase
    {
        public FetcherUpcastMethod(OdcmType baseType, OdcmType derivedType) : base(baseType, derivedType)
        {
            Name = "To" + derivedType.Name;
            Parameters = global::Vipr.Writer.CSharp.Lite.Parameters.Empty;
            ReturnType = new Type(NamesService.GetFetcherInterfaceName(derivedType));
        }
    }
}
