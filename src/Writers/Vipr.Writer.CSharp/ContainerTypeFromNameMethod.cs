// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class ContainerTypeFromNameMethod : Method
    {
        public string ClientNamespace { get; private set; }

        public string ServerNamespace { get; private set; }

        public ContainerTypeFromNameMethod(OdcmClass odcmContainer)
        {
            ClientNamespace = NamesService.GetNamespaceName(odcmContainer.Namespace);
            Visibility = Visibility.Private;
            Name = "ResolveTypeFromName";
            Parameters = new[] { new Parameter(new Type(new Identifier("System", "String")), "typeName"), };
            ReturnType = new Type(new Identifier("global::System", "Type"));
            ServerNamespace = odcmContainer.Namespace.Name;
        }
    }
}