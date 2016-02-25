// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp.Lite
{
    public class ConcreteExecuteAsyncMethod : Method
    {
        public Identifier EntityIdentifier { get; private set; }

        public ConcreteExecuteAsyncMethod(OdcmType odcmType)
        {
            EntityIdentifier = NamesService.GetConcreteInterfaceName(odcmType);
            DefiningInterface = NamesService.GetFetcherInterfaceName(odcmType);
            Name = "ExecuteAsync";
            Parameters = Parameter.Empty;
            ReturnType = new Type(new Identifier("global::System.Threading.Tasks", "Task"),
                 new Type(NamesService.GetConcreteInterfaceName(odcmType)));
        }
    }
}
