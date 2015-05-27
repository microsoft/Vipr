// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp.Lite
{
    public class Class : AttributableStructure
    {
        private Class()
        {
            Constructors = global::Vipr.Writer.CSharp.Lite.Constructors.Empty;
            Fields = global::Vipr.Writer.CSharp.Lite.Fields.Empty;
            Indexers = global::Vipr.Writer.CSharp.Lite.Indexers.Empty;
            Interfaces = global::Vipr.Writer.CSharp.Lite.ImplementedInterfaces.Empty;
            Methods = global::Vipr.Writer.CSharp.Lite.Methods.Empty;
            NestedClasses = global::Vipr.Writer.CSharp.Lite.Classes.Empty;
            Properties = global::Vipr.Writer.CSharp.Lite.Properties.Empty;
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
                Constructors = global::Vipr.Writer.CSharp.Lite.Constructors.ForFetcher(odcmClass),
                Fields = global::Vipr.Writer.CSharp.Lite.Fields.ForFetcher(odcmClass),
                Identifier = NamesService.GetFetcherTypeName(odcmClass),
                Interfaces = global::Vipr.Writer.CSharp.Lite.ImplementedInterfaces.ForFetcherClass(odcmClass),
                Methods = global::Vipr.Writer.CSharp.Lite.Methods.ForFetcherClass(odcmClass),
                Properties = global::Vipr.Writer.CSharp.Lite.Properties.ForFetcher(odcmClass),
            };
        }

        public static Class ForComplex(OdcmClass odcmClass)
        {
            return new Class
            {
                AbstractModifier = odcmClass.IsAbstract ? "abstract " : string.Empty,
                AccessModifier = "public ",
                Constructors = global::Vipr.Writer.CSharp.Lite.Constructors.ForComplex(odcmClass),
                BaseClass =
                    new Type(odcmClass.Base == null
                        ? NamesService.GetExtensionTypeName("ComplexTypeBase")
                        : NamesService.GetPublicTypeName(odcmClass.Base)),
                Description = odcmClass.Description,
                Fields = global::Vipr.Writer.CSharp.Lite.Fields.ForComplex(odcmClass),
                Identifier = NamesService.GetConcreteTypeName(odcmClass),
                Properties = global::Vipr.Writer.CSharp.Lite.Properties.ForComplex(odcmClass),
            };
        }

        public static Class ForConcrete(OdcmEntityClass odcmClass)
        {
            return new Class
            {
                AbstractModifier = odcmClass.IsAbstract ? "abstract " : string.Empty,
                AccessModifier = "public ",
                Attributes = global::Vipr.Writer.CSharp.Lite.Attributes.ForConcrete(odcmClass),
                BaseClass =
                    new Type(odcmClass.Base == null
                        ? NamesService.GetExtensionTypeName("EntityBase")
                        : NamesService.GetConcreteTypeName(odcmClass.Base)),
                Constructors = global::Vipr.Writer.CSharp.Lite.Constructors.ForConcrete(odcmClass),
                Description = odcmClass.Description,
                Fields = global::Vipr.Writer.CSharp.Lite.Fields.ForConcrete(odcmClass),
                Identifier = NamesService.GetConcreteTypeName(odcmClass),
                Interfaces = global::Vipr.Writer.CSharp.Lite.ImplementedInterfaces.ForConcrete(odcmClass),
                Properties = global::Vipr.Writer.CSharp.Lite.Properties.ForConcrete(odcmClass)
            };
        }

        public static Class ForCollection(OdcmEntityClass odcmClass)
        {
            return new Class
            {
                AccessModifier = "internal ",
                BaseClass = new Type(NamesService.GetExtensionTypeName("QueryableSet"),
                                     new Type(NamesService.GetConcreteInterfaceName(odcmClass))),
                Constructors = global::Vipr.Writer.CSharp.Lite.Constructors.ForCollection(odcmClass),
                Interfaces = global::Vipr.Writer.CSharp.Lite.ImplementedInterfaces.ForCollectionClass(odcmClass),
                Identifier = NamesService.GetCollectionTypeName(odcmClass),
                Methods = global::Vipr.Writer.CSharp.Lite.Methods.ForCollectionClass(odcmClass),
                Indexers = global::Vipr.Writer.CSharp.Lite.Indexers.ForCollection(odcmClass)
            };
        }

        public static Class ForCountableCollection(OdcmClass odcmClass)
        {
            return new Class
            {
                AccessModifier = "internal ",
                Identifier = NamesService.GetCollectionTypeName(odcmClass),
                Methods = global::Vipr.Writer.CSharp.Lite.Methods.ForCountableCollectionInterface(odcmClass)
            };
        }

        internal static Class ForEntityContainer(OdcmModel odcmModel, OdcmServiceClass odcmContainer)
        {
            return new Class
            {
                AccessModifier = "public ",
                Constructors = global::Vipr.Writer.CSharp.Lite.Constructors.ForEntityContainer(odcmContainer),
                Description = odcmContainer.Description,
                Fields = global::Vipr.Writer.CSharp.Lite.Fields.ForEntityContainer(odcmContainer),
                Interfaces = global::Vipr.Writer.CSharp.Lite.ImplementedInterfaces.ForEntityContainer(odcmContainer),
                Identifier = NamesService.GetEntityContainerTypeName(odcmContainer),
                NestedClasses = new[] { ForGeneratedEdmModel(odcmModel) },
                Methods = global::Vipr.Writer.CSharp.Lite.Methods.ForEntityContainer(odcmContainer),
                Properties = global::Vipr.Writer.CSharp.Lite.Properties.ForEntityContainer(odcmContainer)
            };
        }

        internal static Class ForGeneratedEdmModel(OdcmModel odcmModel)
        {
            return new Class
            {
                AbstractModifier = "abstract ",
                AccessModifier = "private ",
                Fields = global::Vipr.Writer.CSharp.Lite.Fields.ForGeneratedEdmModel(odcmModel),
                Identifier = new Identifier("", "GeneratedEdmModel"),
                Methods = global::Vipr.Writer.CSharp.Lite.Methods.ForGeneratedEdmModel(),
            };
        }

        public static Class ForFetcherUpcastMethods(OdcmEntityClass odcmClass)
        {
            return new Class
            {
                AccessModifier = "internal ",
                Identifier = NamesService.GetFetcherTypeName(odcmClass),
                Methods = global::Vipr.Writer.CSharp.Lite.Methods.ForFetcherUpcasts(odcmClass),
            };
        }
    }
}