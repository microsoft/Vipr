// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class EnsureQueryMethod : Method
    {
        public Type FetchedTypeInterface { get; private set; }
        public Type FetchedType { get; set; }

        public EnsureQueryMethod(OdcmClass odcmClass)
        {
            IsPublic = false;
            Name = "EnsureQuery";
            FetchedType = new Type(NamesService.GetConcreteTypeName(odcmClass));
            FetchedTypeInterface = new Type(NamesService.GetConcreteInterfaceName(odcmClass));
            ReturnType = new Type(NamesService.GetExtensionTypeName("IReadOnlyQueryableSet"), new Type(NamesService.GetConcreteInterfaceName(odcmClass)));
        }
    }
}
