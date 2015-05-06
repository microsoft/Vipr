// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp.Lite
{
    public class CollectionAddLinkAsyncMethod : Method
    {
        public CollectionAddLinkAsyncMethod(OdcmClass odcmClass)
        {
            Name = "AddLinkAsync";
            Parameters = new[]
            {
                new Parameter(new Type(new Identifier("System", "Object")), "source"),
                new Parameter(new Type(NamesService.GetConcreteInterfaceName(odcmClass)), "target"),
                new Parameter(new Type(NamesService.GetPrimitiveTypeName("Boolean")), "deferSaveChanges", "false")
            };
            ReturnType = new Type(Identifier.Task);
        }
    }
}
