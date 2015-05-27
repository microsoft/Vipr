// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp.Lite
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

        public static IEnumerable<Interface> ForConcrete(OdcmClass odcmClass)
        {

            var @interface =  new Interface
            {
                Attributes = global::Vipr.Writer.CSharp.Lite.Attributes.ForConcreteInterface,
                Identifier = NamesService.GetConcreteInterfaceName(odcmClass),
                Description = odcmClass.Description,
                Namespace = NamesService.GetNamespaceName(odcmClass.Namespace),
                Properties = global::Vipr.Writer.CSharp.Lite.Properties.ForConcreteInterface(odcmClass),
                Interfaces = global::Vipr.Writer.CSharp.Lite.ImplementedInterfaces.ForConcreteInterface(odcmClass)
            };

            return new List<Interface>() { @interface };
        }

        public static IEnumerable<Interface> ForFetcher(OdcmClass odcmClass)
        {
            var interfaces = new List<Interface>();
            foreach (var projection in odcmClass.Projections)
            {
                var @interface = new Interface
                {
                    Attributes = global::Vipr.Writer.CSharp.Lite.Attributes.ForFetcherInterface,
                    Identifier = NamesService.GetFetcherInterfaceName(odcmClass, projection),
                    Interfaces = global::Vipr.Writer.CSharp.Lite.ImplementedInterfaces.ForFetcherInterface(odcmClass),
                    Methods = global::Vipr.Writer.CSharp.Lite.Methods.ForFetcherInterface(odcmClass, projection),
                    Namespace = NamesService.GetNamespaceName(odcmClass.Namespace),
                    Properties = global::Vipr.Writer.CSharp.Lite.Properties.ForFetcherInterface(odcmClass)
                };

                interfaces.Add(@interface);
            }

            return interfaces;
        }

        public static IEnumerable<Interface> ForCollection(OdcmEntityClass odcmClass)
        {
            var interfaces = new List<Interface>();
            foreach (var projection in odcmClass.Projections)
            {
                var @interface = new Interface
                {
                    Attributes = global::Vipr.Writer.CSharp.Lite.Attributes.ForCollectionInterface,
                    Identifier = NamesService.GetCollectionInterfaceName(odcmClass, projection),
                    Namespace = NamesService.GetNamespaceName(odcmClass.Namespace),
                    Methods = global::Vipr.Writer.CSharp.Lite.Methods.ForCollectionInterface(odcmClass, projection),
                    Indexers = IndexerSignature.ForCollectionInterface(odcmClass, projection),
                    Interfaces = new[] { new Type(NamesService.GetExtensionTypeName("IReadOnlyQueryableSetBase"), new Type(NamesService.GetConcreteInterfaceName(odcmClass))) }
                };

                interfaces.Add(@interface);
            }

            return interfaces;
        }

        public static Interface ForEntityContainer(OdcmClass odcmContainer)
        {
            return new Interface
            {
                Identifier = NamesService.GetEntityContainerInterfaceName(odcmContainer),
                Description = odcmContainer.Description,
                Interfaces = null,
                Methods = global::Vipr.Writer.CSharp.Lite.Methods.ForEntityContainerInterface(odcmContainer),
                Properties = global::Vipr.Writer.CSharp.Lite.Properties.ForEntityContainerInterface(odcmContainer),
                Namespace = NamesService.GetNamespaceName(odcmContainer.Namespace),
            };
        }

        public static Interface ForCountableCollection(OdcmClass odcmClass)
        {
            return new Interface
            {
                Identifier = NamesService.GetCollectionInterfaceName(odcmClass),
                Methods = global::Vipr.Writer.CSharp.Lite.Methods.ForCountableCollectionInterface(odcmClass)
            };
        }

        public static Interface ForFetcherUpcastMethods(OdcmEntityClass odcmClass)
        {
            return new Interface
            {
                Identifier = NamesService.GetFetcherInterfaceName(odcmClass),
                Methods = global::Vipr.Writer.CSharp.Lite.Methods.ForFetcherInterfaceUpcasts(odcmClass),
            };
        }
    }
}
