// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class Class : AttributableStructure
    {
        private Class()
        {
            Constructors = global::CSharpWriter.Constructors.Empty;
            Fields = global::CSharpWriter.Fields.Empty;
            Indexers = global::CSharpWriter.Indexers.Empty;
            Interfaces = global::CSharpWriter.ImplementedInterfaces.Empty;
            Methods = global::CSharpWriter.Methods.Empty;
            NestedClasses = global::CSharpWriter.Classes.Empty;
            Properties = global::CSharpWriter.Properties.Empty;
        }

        public string AbstractModifier { get; private set; }
        public string AccessModifier { get; private set; }
        public Type BaseClass { get; private set; }
        public string Description { get; private set; }
        public IEnumerable<Field> Fields { get; private set; }
        public Identifier Identifier { get; private set; }
        public IEnumerable<Indexer> Indexers { get; private set; }
        public IEnumerable<Type> Interfaces { get; private set; }
        public IEnumerable<Method> Methods { get; private set; }
        public IEnumerable<Property> Properties { get; private set; }
        public IEnumerable<Constructor> Constructors { get; private set; }
        public IEnumerable<Class> NestedClasses { get; private set; }

        public static Class ForFetcher(OdcmClass odcmClass)
        {
            return new Class
            {
                AccessModifier = "internal ",
                BaseClass =
                    new Type(odcmClass.Base == null
                        ? NamesService.GetExtensionTypeName("RestShallowObjectFetcher")
                        : NamesService.GetFetcherTypeName(odcmClass.Base)),
                Constructors = global::CSharpWriter.Constructors.ForFetcher(odcmClass),
                Fields = global::CSharpWriter.Fields.ForFetcher(odcmClass),
                Identifier = NamesService.GetFetcherTypeName(odcmClass),
                Interfaces = global::CSharpWriter.ImplementedInterfaces.ForFetcher(odcmClass),
                Methods = global::CSharpWriter.Methods.ForFetcher(odcmClass),
                Properties = global::CSharpWriter.Properties.ForFetcher(odcmClass),
            };
        }

        public static Class ForComplex(OdcmClass odcmClass)
        {
            return new Class
            {
                AbstractModifier = odcmClass.IsAbstract ? "abstract " : string.Empty,
                AccessModifier = "public ",
                Constructors = global::CSharpWriter.Constructors.ForComplex(odcmClass),
                BaseClass =
                    new Type(odcmClass.Base == null
                        ? NamesService.GetExtensionTypeName("ComplexTypeBase")
                        : NamesService.GetPublicTypeName(odcmClass.Base)),
                Description = odcmClass.Description,
                Fields = global::CSharpWriter.Fields.ForComplex(odcmClass),
                Identifier = NamesService.GetConcreteTypeName(odcmClass),
                Properties = global::CSharpWriter.Properties.ForComplex(odcmClass),
            };
        }

        public static Class ForConcrete(OdcmEntityClass odcmClass)
        {
            return new Class
            {
                AbstractModifier = odcmClass.IsAbstract ? "abstract " : string.Empty,
                AccessModifier = "public ",
                Attributes = global::CSharpWriter.Attributes.ForConcrete(odcmClass),
                BaseClass =
                    new Type(odcmClass.Base == null
                        ? NamesService.GetExtensionTypeName("EntityBase")
                        : NamesService.GetConcreteTypeName(odcmClass.Base)),
                Constructors = global::CSharpWriter.Constructors.ForConcrete(odcmClass),
                Description = odcmClass.Description,
                Fields = global::CSharpWriter.Fields.ForConcrete(odcmClass),
                Identifier = NamesService.GetConcreteTypeName(odcmClass),
                Interfaces = global::CSharpWriter.ImplementedInterfaces.ForConcrete(odcmClass),
                Methods = global::CSharpWriter.Methods.ForConcrete(odcmClass),
                Properties = global::CSharpWriter.Properties.ForConcrete(odcmClass)
            };
        }

        public static Class ForCollection(OdcmEntityClass odcmClass)
        {
            return new Class
            {
                AccessModifier = "internal ",
                BaseClass = new Type(NamesService.GetExtensionTypeName("QueryableSet"),
                                     new Type(NamesService.GetConcreteInterfaceName(odcmClass))),
                Constructors = global::CSharpWriter.Constructors.ForCollection(odcmClass),
                Interfaces = global::CSharpWriter.ImplementedInterfaces.ForCollection(odcmClass),
                Identifier = NamesService.GetCollectionTypeName(odcmClass),
                Methods = global::CSharpWriter.Methods.ForCollection(odcmClass),
                Indexers = global::CSharpWriter.Indexers.ForCollection(odcmClass)
            };
        }

        public static Class ForCountableCollection(OdcmClass odcmClass)
        {
            return new Class
            {
                AccessModifier = "internal ",
                Identifier = NamesService.GetCollectionTypeName(odcmClass),
                Methods = global::CSharpWriter.Methods.ForCountableCollectionInterface(odcmClass)
            };
        }

        internal static Class ForEntityContainer(OdcmModel odcmModel, OdcmServiceClass odcmContainer)
        {
            return new Class
            {
                AccessModifier = "public ",
                Constructors = global::CSharpWriter.Constructors.ForEntityContainer(odcmContainer),
                Description = odcmContainer.Description,
                Fields = global::CSharpWriter.Fields.ForEntityContainer(odcmContainer),
                Interfaces = global::CSharpWriter.ImplementedInterfaces.ForEntityContainer(odcmContainer),
                Identifier = NamesService.GetEntityContainerTypeName(odcmContainer),
                NestedClasses = new[] { ForGeneratedEdmModel(odcmModel) },
                Methods = global::CSharpWriter.Methods.ForEntityContainer(odcmContainer),
                Properties = global::CSharpWriter.Properties.ForEntityContainer(odcmContainer)
            };
        }

        internal static Class ForGeneratedEdmModel(OdcmModel odcmModel)
        {
            return new Class
            {
                AbstractModifier = "abstract ",
                AccessModifier = "private ",
                Fields = global::CSharpWriter.Fields.ForGeneratedEdmModel(odcmModel),
                Identifier = new Identifier("", "GeneratedEdmModel"),
                Methods = global::CSharpWriter.Methods.ForGeneratedEdmModel(),
            };
        }

        public static Class ForFetcherUpcastMethods(OdcmEntityClass odcmClass)
        {
            return new Class
            {
                AccessModifier = "internal ",
                Identifier = NamesService.GetFetcherTypeName(odcmClass),
                Methods = global::CSharpWriter.Methods.ForFetcherUpcasts(odcmClass),
            };
        }

        public static Class ForConcreteIFetcherUpcastMethods(OdcmEntityClass odcmClass)
        {
            return new Class
            {
                AccessModifier = "public ",
                Identifier = NamesService.GetConcreteTypeName(odcmClass),
                Methods = global::CSharpWriter.Methods.ForConcreteUpcasts(odcmClass),
            };
        }
    }
}