// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class ContainerNameFromTypeMethod : Method
    {
        public string ClientNamespace { get; private set; }

        public string ServerNamespace { get; private set; }

        public ContainerNameFromTypeMethod(OdcmClass odcmContainer)
        {
            ClientNamespace = odcmContainer.Namespace;
            IsPublic = false;
            Name = "ResolveNameFromType";
            Parameters = new[] { new Parameter(new Type(new Identifier("global::System", "Type")), "clientType"), };
            ReturnType = new Type(new Identifier("System", "String"));
            ServerNamespace = odcmContainer.Namespace;
        }
    }
}