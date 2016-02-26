// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace CSharpLiteWriterUnitTests
{
    public static class TypeKindExtensions
    {
        internal static TypeKinds GetTypeKind(this Type type)
        {
            if (type.IsPrimitive)
                return TypeKinds.Primitive;
            if (type.IsClass)
                return TypeKinds.Class;
            if (type.IsInterface)
                return TypeKinds.Interface;

            return TypeKinds.None;
        }
    }
}