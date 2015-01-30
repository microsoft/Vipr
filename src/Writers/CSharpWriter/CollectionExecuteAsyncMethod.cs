// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class CollectionExecuteAsyncMethod : Method
    {
        public CollectionExecuteAsyncMethod(OdcmClass odcmClass)
        {
            Name = "ExecuteAsync";
            Parameters = Parameter.Empty;
            ReturnType = new Type(new Identifier("global::System.Threading.Tasks", "Task"),
                new Type(NamesService.GetExtensionTypeName("IPagedCollection"),
                    new Type(NamesService.GetConcreteInterfaceName(odcmClass))));
        }
    }
}
