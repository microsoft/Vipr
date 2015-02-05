// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class Field
    {
        private Field()
        {
        }

        public Field(string name, Type type, string defaultValue = "", bool isConstant = false, bool isStatic = false)
        {
            Name = name;
            Type = type;
            IsConstant = isConstant;
            IsStatic = isStatic;
            DefaultValue = defaultValue;
        }

        public string Name { get; private set; }
        public Type Type { get; private set; }
        public bool IsConstant { get; private set; }
        public bool IsStatic { get; private set; }
        public string DefaultValue { get; private set; }

        public static Field ForNavigationProperty(OdcmProperty property)
        {
            return new Field
            {
                Name = NamesService.GetPropertyFieldName(property),
                Type = new Type(NamesService.GetConcreteTypeName(property.Type))
            };
        }

        public static Field ForStructuralProperty(OdcmProperty property)
        {
            return new Field
            {
                Name = NamesService.GetPropertyFieldName(property),
                Type = property.IsCollection
                    ? new Type(NamesService.GetExtensionTypeName("NonEntityTypeCollectionImpl"), new Type(NamesService.GetConcreteTypeName(property.Type)))
                    : TypeService.GetPropertyType(property)
            };
        }

        public static Field ForNavigationFetcherProperty(OdcmProperty property)
        {
            return new Field
            {
                Name = NamesService.GetFetcherFieldName(property),
                Type = property.IsCollection
                     ? new Type(NamesService.GetCollectionTypeName((OdcmClass)property.Type))
                     : new Type(NamesService.GetFetcherTypeName(property.Type))
            };
        }

        public static Field ForNavigationCollectionProperty(OdcmProperty property)
        {
            return new Field
            {
                Name = NamesService.GetFetcherCollectionFieldName(property),
                Type = new Type(NamesService.GetCollectionTypeName((OdcmClass)property.Type))
            };
        }

        public static Field ForNavigationConcreteProperty(OdcmProperty property)
        {
            return new Field
            {
                Name = NamesService.GetConcreteFieldName(property),
                Type = new Type(NamesService.GetExtensionTypeName("EntityCollectionImpl"), new Type(NamesService.GetConcreteTypeName((OdcmClass)property.Type)))
            };
        }
    }
}
