// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class ConcreteUpcastMethod : FetcherUpcastMethod
    {
        public ConcreteUpcastMethod(OdcmType baseType, OdcmType derivedType) : base(baseType, derivedType)
        {
            DefiningInterface = NamesService.GetFetcherInterfaceName(baseType);
        }
    }
}
