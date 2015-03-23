// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    internal class ContainerAddToCollectionMethod : Method
    {
        public string ModelCollectionName { get; private set; }

        public ContainerAddToCollectionMethod(OdcmProperty odcmProperty)
        {
            Name = "AddTo" + odcmProperty.Name;

            ModelCollectionName = odcmProperty.Name;

            Parameters = new[]
            {
                new Parameter(new Type(NamesService.GetConcreteInterfaceName(odcmProperty.Projection.Type)), odcmProperty.Name.ToLowerCamelCase()),
            };

            ReturnType = Type.Void;
        }
    }
}