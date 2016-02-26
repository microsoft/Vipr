// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;

namespace CSharpLiteWriterUnitTests
{
    public static class CSharpAccessModifierExtensions
    {
        internal static CSharpAccessModifiers GetCSharpAccessModifier(this MethodInfo methodInfo)
        {
            if (methodInfo.IsPrivate)
                return CSharpAccessModifiers.Private;
            if (methodInfo.IsFamily)
                return CSharpAccessModifiers.Protected;
            if (methodInfo.IsAssembly)
                return CSharpAccessModifiers.Internal;
            if (methodInfo.IsPublic)
                return CSharpAccessModifiers.Public;
            if (methodInfo.IsFamilyOrAssembly)
                return CSharpAccessModifiers.ProtectedInternal;

            return CSharpAccessModifiers.None;
        }

        internal static CSharpAccessModifiers GetCSharpAccessModifier(this ConstructorInfo constructorInfo)
        {
            if (constructorInfo.IsPrivate)
                return CSharpAccessModifiers.Private;
            if (constructorInfo.IsFamily)
                return CSharpAccessModifiers.Protected;
            if (constructorInfo.IsAssembly)
                return CSharpAccessModifiers.Internal;
            if (constructorInfo.IsPublic)
                return CSharpAccessModifiers.Public;
            if (constructorInfo.IsFamilyOrAssembly)
                return CSharpAccessModifiers.ProtectedInternal;

            return CSharpAccessModifiers.None;
        }

        internal static CSharpAccessModifiers GetCSharpAccessModifier(this Type type)
        {
            if (type.IsNestedPrivate)
                return CSharpAccessModifiers.Private;
            if (type.IsNestedFamily)
                return CSharpAccessModifiers.Protected;
            if (type.IsNestedAssembly)
                return CSharpAccessModifiers.Internal;
            if (type.IsPublic)
                return CSharpAccessModifiers.Public;
            if (type.IsNestedFamORAssem)
                return CSharpAccessModifiers.ProtectedInternal;

            return CSharpAccessModifiers.None;
        }
    }
}