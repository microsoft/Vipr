// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    internal class Interface : AttributableStructure
    {
        public Identifier Identifier { get; private set; }

        public string Namespace { get; private set; }

        public IEnumerable<InterfaceProperty> Properties { get; private set; }

        public IEnumerable<IndexerSignature> Indexers { get; private set; }

        public IEnumerable<Type> Interfaces { get; private set; }

        public IEnumerable<MethodSignature> Methods { get; private set; }

        private Interface()
        {
            Indexers = new List<Indexer>();
            Interfaces = new List<Type>();
            Methods = new List<Method>();
            Properties = new List<InterfaceProperty>();
        }

        public static Interface ForConcrete(OdcmClass odcmClass)
        {
            return new Interface
            {
                Attributes = global::CSharpWriter.Attributes.ForConcreteInterface,
                Identifier = NamesService.GetConcreteInterfaceName(odcmClass),
                Methods = global::CSharpWriter.Methods.ForConcreteInterface(odcmClass),
                Namespace = odcmClass.Namespace,
                Properties = global::CSharpWriter.Properties.ForConcreteInterface(odcmClass),
                Interfaces = global::CSharpWriter.ImplementedInterfaces.ForConcreteInterface(odcmClass)
            };
        }

        public static Interface ForFetcher(OdcmClass odcmClass)
        {
            return new Interface
            {
                Attributes = global::CSharpWriter.Attributes.ForFetcherInterface,
                Identifier = NamesService.GetFetcherInterfaceName(odcmClass),
                Interfaces = global::CSharpWriter.ImplementedInterfaces.ForFetcherInterface(odcmClass),
                Methods = global::CSharpWriter.Methods.ForFetcherInterface(odcmClass),
                Namespace = odcmClass.Namespace,
                Properties = global::CSharpWriter.Properties.ForFetcherInterface(odcmClass)
            };
        }

        public static Interface ForCollection(OdcmEntityClass odcmClass)
        {
            return new Interface
            {
                Attributes = global::CSharpWriter.Attributes.ForCollectionInterface,
                Identifier = NamesService.GetCollectionInterfaceName(odcmClass),
                Namespace = odcmClass.Namespace,
                Methods = global::CSharpWriter.Methods.ForCollectionInterface(odcmClass),
                Indexers = IndexerSignature.ForCollectionInterface(odcmClass),
                Interfaces = new[] { new Type(NamesService.GetExtensionTypeName("IReadOnlyQueryableSetBase"), new Type(NamesService.GetConcreteInterfaceName(odcmClass))) }
            };
        }

        public static Interface ForEntityContainer(OdcmClass odcmContainer)
        {
            return new Interface
            {
                Identifier = NamesService.GetEntityContainerInterfaceName(odcmContainer),
                Interfaces = null,
                Methods = global::CSharpWriter.Methods.ForEntityContainerInterface(odcmContainer),
                Properties = global::CSharpWriter.Properties.ForEntityContainerInterface(odcmContainer),
                Namespace = odcmContainer.Namespace
            };
        }

        public static Interface ForCountableCollection(OdcmClass odcmClass)
        {
            return new Interface
            {
                Identifier = NamesService.GetCollectionInterfaceName(odcmClass),
                Methods = global::CSharpWriter.Methods.ForCountableCollectionInterface(odcmClass)
            };
        }
    }
}
