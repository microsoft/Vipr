// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CSharpWriter
{
    public class Identifier
    {
        public Identifier(string @namespace, string name)
        {
            Name = name;
            Namespace = @namespace;
        }

        public string Name { get; private set; }

        public string Namespace { get; private set; }

        public string FullName
        {
            get
            {
                return Namespace == null
                    ? Name
                    : Namespace + "." + Name;
            }
        }

        public override string ToString()
        {
            return FullName;
        }


        public static Identifier Task { get { return new Identifier("global::System.Threading.Tasks", "Task"); } }
    }
}
