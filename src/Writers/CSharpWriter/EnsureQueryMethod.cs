// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class EnsureQueryMethod : Method
    {
        public Type QueryableSetType { get; private set; }
        public Identifier FetchedType { get; set; }

        public EnsureQueryMethod(OdcmClass odcmClass)
        {
            IsPublic = false;
            Name = "EnsureQuery";
            FetchedType = NamesService.GetPublicTypeName(odcmClass);
            QueryableSetType = new Type(NamesService.GetExtensionTypeName("ReadOnlyQueryableSet"), new Type(NamesService.GetPublicTypeName(odcmClass)));
            ReturnType = new Type(NamesService.GetExtensionTypeName("IReadOnlyQueryableSet"), new Type(NamesService.GetConcreteInterfaceName(odcmClass)));
        }
    }
}
