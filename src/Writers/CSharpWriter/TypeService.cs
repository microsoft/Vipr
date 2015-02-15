// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public static class TypeService
    {
        public static bool IsValueType(OdcmType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return
                type is OdcmEnum ||
                (
                    type is OdcmPrimitiveType &&
                    type.Namespace.Equals("Edm", StringComparison.OrdinalIgnoreCase) &&
                    (
                        type.Name.Equals("Boolean", StringComparison.OrdinalIgnoreCase) ||
                        type.Name.Equals("Byte", StringComparison.OrdinalIgnoreCase) ||
                        type.Name.Equals("Date", StringComparison.OrdinalIgnoreCase) ||
                        type.Name.Equals("DateTimeOffset", StringComparison.OrdinalIgnoreCase) ||
                        type.Name.Equals("Decimal", StringComparison.OrdinalIgnoreCase) ||
                        type.Name.Equals("Double", StringComparison.OrdinalIgnoreCase) ||
                        type.Name.Equals("Duration", StringComparison.OrdinalIgnoreCase) ||
                        type.Name.Equals("Int16", StringComparison.OrdinalIgnoreCase) ||
                        type.Name.Equals("Int32", StringComparison.OrdinalIgnoreCase) ||
                        type.Name.Equals("Int64", StringComparison.OrdinalIgnoreCase) ||
                        type.Name.Equals("SByte", StringComparison.OrdinalIgnoreCase) ||
                        type.Name.Equals("Single", StringComparison.OrdinalIgnoreCase) ||
                        type.Name.Equals("Guid", StringComparison.OrdinalIgnoreCase) ||
                        type.Name.Equals("TimeOfDay", StringComparison.OrdinalIgnoreCase)
                    )
                );
        }

        public static Type GetNullableType(OdcmType type)
        {
            return new Type(new Identifier("System", "Nullable"), new Type(NamesService.GetPublicTypeName(type)));
        }

        public static Type GetPropertyType(OdcmProperty property)
        {
            return property.IsNullable && IsValueType(property.Type) && !(property.Type is OdcmEnum)
                ? GetNullableType(property.Type)
                : new Type(NamesService.GetPublicTypeName(property.Type));
        }

        public static Type GetParameterType(OdcmParameter parameter)
        {
            return parameter.IsNullable && IsValueType(parameter.Type) && !(parameter.Type is OdcmEnum)
                ? GetNullableType(parameter.Type)
                : new Type(NamesService.GetPublicTypeName(parameter.Type));
        }
    }
}