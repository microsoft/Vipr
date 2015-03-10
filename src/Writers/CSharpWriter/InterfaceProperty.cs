// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CSharpWriter
{
    public class InterfaceProperty : ExplicitlyImplementableMember
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public Type Type { get; protected set; }
        public bool PrivateGet = false;
        public bool PrivateSet = false;
    }
}
