// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp
{
    internal class Interface : AttributableStructure
    {
        public Identifier Identifier { get; private set; }
        public string Description { get; private set; }

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
                Attributes = global::Vipr.Writer.CSharp.Attributes.ForConcreteInterface,
                Identifier = NamesService.GetConcreteInterfaceName(odcmClass),
                Description = odcmClass.Description,
                Methods = global::Vipr.Writer.CSharp.Methods.ForConcreteInterface(odcmClass),
                Namespace = NamesService.GetNamespaceName(odcmClass.Namespace),
                Properties = global::Vipr.Writer.CSharp.Properties.ForConcreteInterface(odcmClass),
                Interfaces = global::Vipr.Writer.CSharp.ImplementedInterfaces.ForConcreteInterface(odcmClass)
            };
        }

        public static Interface ForFetcher(OdcmClass odcmClass)
        {
            return new Interface
            {
                Attributes = global::Vipr.Writer.CSharp.Attributes.ForFetcherInterface,
                Identifier = NamesService.GetFetcherInterfaceName(odcmClass),
                Interfaces = global::Vipr.Writer.CSharp.ImplementedInterfaces.ForFetcherInterface(odcmClass),
                Methods = global::Vipr.Writer.CSharp.Methods.ForFetcherInterface(odcmClass),
                Namespace = NamesService.GetNamespaceName(odcmClass.Namespace),
                Properties = global::Vipr.Writer.CSharp.Properties.ForFetcherInterface(odcmClass)
            };
        }

        public static Interface ForCollection(OdcmEntityClass odcmClass)
        {
            return new Interface
            {
                Attributes = global::Vipr.Writer.CSharp.Attributes.ForCollectionInterface,
                Identifier = NamesService.GetCollectionInterfaceName(odcmClass),
                Namespace = NamesService.GetNamespaceName(odcmClass.Namespace),
                Methods = global::Vipr.Writer.CSharp.Methods.ForCollectionInterface(odcmClass),
                Indexers = IndexerSignature.ForCollectionInterface(odcmClass),
                Interfaces = new[] { new Type(NamesService.GetExtensionTypeName("IReadOnlyQueryableSetBase"), new Type(NamesService.GetConcreteInterfaceName(odcmClass))) }
            };
        }

        public static Interface ForEntityContainer(OdcmClass odcmContainer)
        {
            return new Interface
            {
                Identifier = NamesService.GetEntityContainerInterfaceName(odcmContainer),
                Description = odcmContainer.Description,
                Interfaces = null,
                Methods = global::Vipr.Writer.CSharp.Methods.ForEntityContainerInterface(odcmContainer),
                Properties = global::Vipr.Writer.CSharp.Properties.ForEntityContainerInterface(odcmContainer),
                Namespace = NamesService.GetNamespaceName(odcmContainer.Namespace),
            };
        }

        public static Interface ForCountableCollection(OdcmClass odcmClass)
        {
            return new Interface
            {
                Identifier = NamesService.GetCollectionInterfaceName(odcmClass),
                Methods = global::Vipr.Writer.CSharp.Methods.ForCountableCollectionInterface(odcmClass)
            };
        }

        public static Interface ForFetcherUpcastMethods(OdcmEntityClass odcmClass)
        {
            return new Interface
            {
                Identifier = NamesService.GetFetcherInterfaceName(odcmClass),
                Methods = global::Vipr.Writer.CSharp.Methods.ForFetcherInterfaceUpcasts(odcmClass),
            };
        }
    }
}
