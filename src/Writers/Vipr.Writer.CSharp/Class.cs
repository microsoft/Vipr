// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp
{
    public class Class : AttributableStructure
    {
        private Class()
        {
            Constructors = global::Vipr.Writer.CSharp.Constructors.Empty;
            Fields = global::Vipr.Writer.CSharp.Fields.Empty;
            Indexers = global::Vipr.Writer.CSharp.Indexers.Empty;
            Interfaces = global::Vipr.Writer.CSharp.ImplementedInterfaces.Empty;
            Methods = global::Vipr.Writer.CSharp.Methods.Empty;
            NestedClasses = global::Vipr.Writer.CSharp.Classes.Empty;
            Properties = global::Vipr.Writer.CSharp.Properties.Empty;
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
                Constructors = global::Vipr.Writer.CSharp.Constructors.ForFetcher(odcmClass),
                Fields = global::Vipr.Writer.CSharp.Fields.ForFetcher(odcmClass),
                Identifier = NamesService.GetFetcherTypeName(odcmClass),
                Interfaces = global::Vipr.Writer.CSharp.ImplementedInterfaces.ForFetcher(odcmClass),
                Methods = global::Vipr.Writer.CSharp.Methods.ForFetcher(odcmClass),
                Properties = global::Vipr.Writer.CSharp.Properties.ForFetcher(odcmClass),
            };
        }

        public static Class ForComplex(OdcmClass odcmClass)
        {
            return new Class
            {
                AbstractModifier = odcmClass.IsAbstract ? "abstract " : string.Empty,
                AccessModifier = "public ",
                Constructors = global::Vipr.Writer.CSharp.Constructors.ForComplex(odcmClass),
                BaseClass =
                    new Type(odcmClass.Base == null
                        ? NamesService.GetExtensionTypeName("ComplexTypeBase")
                        : NamesService.GetPublicTypeName(odcmClass.Base)),
                Description = odcmClass.Description,
                Fields = global::Vipr.Writer.CSharp.Fields.ForComplex(odcmClass),
                Identifier = NamesService.GetConcreteTypeName(odcmClass),
                Properties = global::Vipr.Writer.CSharp.Properties.ForComplex(odcmClass),
            };
        }

        public static Class ForConcrete(OdcmEntityClass odcmClass)
        {
            return new Class
            {
                AbstractModifier = odcmClass.IsAbstract ? "abstract " : string.Empty,
                AccessModifier = "public ",
                Attributes = global::Vipr.Writer.CSharp.Attributes.ForConcrete(odcmClass),
                BaseClass =
                    new Type(odcmClass.Base == null
                        ? NamesService.GetExtensionTypeName("EntityBase")
                        : NamesService.GetConcreteTypeName(odcmClass.Base)),
                Constructors = global::Vipr.Writer.CSharp.Constructors.ForConcrete(odcmClass),
                Description = odcmClass.Description,
                Fields = global::Vipr.Writer.CSharp.Fields.ForConcrete(odcmClass),
                Identifier = NamesService.GetConcreteTypeName(odcmClass),
                Interfaces = global::Vipr.Writer.CSharp.ImplementedInterfaces.ForConcrete(odcmClass),
                Methods = global::Vipr.Writer.CSharp.Methods.ForConcrete(odcmClass),
                Properties = global::Vipr.Writer.CSharp.Properties.ForConcrete(odcmClass)
            };
        }

        public static Class ForCollection(OdcmEntityClass odcmClass)
        {
            return new Class
            {
                AccessModifier = "internal ",
                BaseClass = new Type(NamesService.GetExtensionTypeName("QueryableSet"),
                                     new Type(NamesService.GetConcreteInterfaceName(odcmClass))),
                Constructors = global::Vipr.Writer.CSharp.Constructors.ForCollection(odcmClass),
                Interfaces = global::Vipr.Writer.CSharp.ImplementedInterfaces.ForCollection(odcmClass),
                Identifier = NamesService.GetCollectionTypeName(odcmClass),
                Methods = global::Vipr.Writer.CSharp.Methods.ForCollection(odcmClass),
                Indexers = global::Vipr.Writer.CSharp.Indexers.ForCollection(odcmClass)
            };
        }

        public static Class ForCountableCollection(OdcmClass odcmClass)
        {
            return new Class
            {
                AccessModifier = "internal ",
                Identifier = NamesService.GetCollectionTypeName(odcmClass),
                Methods = global::Vipr.Writer.CSharp.Methods.ForCountableCollectionInterface(odcmClass)
            };
        }

        internal static Class ForEntityContainer(OdcmModel odcmModel, OdcmServiceClass odcmContainer)
        {
            return new Class
            {
                AccessModifier = "public ",
                Constructors = global::Vipr.Writer.CSharp.Constructors.ForEntityContainer(odcmContainer),
                Description = odcmContainer.Description,
                Fields = global::Vipr.Writer.CSharp.Fields.ForEntityContainer(odcmContainer),
                Interfaces = global::Vipr.Writer.CSharp.ImplementedInterfaces.ForEntityContainer(odcmContainer),
                Identifier = NamesService.GetEntityContainerTypeName(odcmContainer),
                NestedClasses = new[] { ForGeneratedEdmModel(odcmModel) },
                Methods = global::Vipr.Writer.CSharp.Methods.ForEntityContainer(odcmContainer),
                Properties = global::Vipr.Writer.CSharp.Properties.ForEntityContainer(odcmContainer)
            };
        }

        internal static Class ForGeneratedEdmModel(OdcmModel odcmModel)
        {
            return new Class
            {
                AbstractModifier = "abstract ",
                AccessModifier = "private ",
                Fields = global::Vipr.Writer.CSharp.Fields.ForGeneratedEdmModel(odcmModel),
                Identifier = new Identifier("", "GeneratedEdmModel"),
                Methods = global::Vipr.Writer.CSharp.Methods.ForGeneratedEdmModel(),
            };
        }

        public static Class ForFetcherUpcastMethods(OdcmEntityClass odcmClass)
        {
            return new Class
            {
                AccessModifier = "internal ",
                Identifier = NamesService.GetFetcherTypeName(odcmClass),
                Methods = global::Vipr.Writer.CSharp.Methods.ForFetcherUpcasts(odcmClass),
            };
        }

        public static Class ForConcreteIFetcherUpcastMethods(OdcmEntityClass odcmClass)
        {
            return new Class
            {
                AccessModifier = "public ",
                Identifier = NamesService.GetConcreteTypeName(odcmClass),
                Methods = global::Vipr.Writer.CSharp.Methods.ForConcreteUpcasts(odcmClass),
            };
        }
    }
}