// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class Type
    {
        public static Type Void { get { return s_void; } }

        private static Type s_void = new Type();

        public Identifier TypeIdentifier { get; private set; }

        public IEnumerable<Type> GenericParameters { get; private set; }

        private Type()
        {
        }

        public Type(Identifier typeIdentifier, params Type[] genericParameters)
        {
            GenericParameters = genericParameters;
            TypeIdentifier = typeIdentifier;
        }

        public override string ToString()
        {
            if (this == Void)
                return "void";

            var genericParameterString = String.Join(", ", GenericParameters);

            if (genericParameterString.Length > 0)
                genericParameterString = string.Format("<{0}>", genericParameterString);

            return String.Format("{0}{1}", TypeIdentifier, genericParameterString);
        }

        public static Type TaskOf(Type type)
        {
            return new Type(new Identifier("System.Threading.Tasks", "Task"), type);
        }

        public static Type IEnmerableOf(Type type)
        {
            return new Type(new Identifier("System.Collections.Generic", "IEnumerable"), type);
        }
    }
}
