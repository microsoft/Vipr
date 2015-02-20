// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    internal class CollectionCountAsyncMethod : Method
    {

        public CollectionCountAsyncMethod()
        {
            Name = "CountAsync";

            Parameters = new Parameter[]
            {
            };

            ReturnType = new Type(Identifier.Task, new[] { new Type(NamesService.GetPrimitiveTypeName("Int64")), });
        }
    }
}