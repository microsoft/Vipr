// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSharpWriterUnitTests
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<string> GetNamespaces(this Assembly asm)
        {
            return asm.GetTypes().Select(t => t.Namespace).Where(n => n != null).Distinct();
        }

        public static IEnumerable<EnumType> GetEnums(this Assembly asm)
        {
            return asm.GetTypes().Where(t => t.IsEnum).Select(t => new EnumType(t));
        }

        public static EnumType GetEnum(this Assembly asm, string @namespace, string name)
        {
            return asm.GetEnums().FirstOrDefault(e => e.Name == name && e.Namespace == @namespace);
        }

        public static IEnumerable<Type> GetClasses(this Assembly asm)
        {
            return asm.GetTypes().Where(t => t.IsClass);
        }

        public static Type GetClass(this Assembly asm, string @namespace, string name)
        {
            return @namespace == "Edm" ?
                CodeGenTestBase.EdmToClrTypeMap[name] :
                asm.GetClasses().FirstOrDefault(e => e.Name == name && e.Namespace == @namespace);
        }

        public static Type GetInterface(this Assembly asm, string @namespace, string name)
        {
            return asm.GetTypes().FirstOrDefault(e => e.Name == name && e.Namespace == @namespace && e.IsInterface);
        }

        public static Type GetClass(this Assembly asm, string name)
        {
            return asm.GetClasses().FirstOrDefault(e => e.Name == name);
        }

        public static bool IsInternal(this Type type)
        {
            return type.IsNotPublic && !type.IsNested;
        }

        public static Type MakeGenericType(this Assembly asm, string @namespace, string name,
            params Type[] genericArguments)
        {
            var typeName = String.Format("{0}`{1}", name, genericArguments.Count());

            return asm.GetTypes()
                .FirstOrDefault(t => (@namespace == null || t.Namespace == @namespace)
                                     && t.Name == typeName
                                     && t.IsGenericTypeDefinition)
                .MakeGenericType(genericArguments);
        }
    }
}
