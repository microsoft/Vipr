// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CSharpWriter
{
    public class AutoProperty : Property
    {
        public AutoProperty(string name, Type type, bool isPublic = true, bool privateGet = false, bool privateSet = false) : base(name)
        {
            IsPublic = isPublic;
            Name = name;
            PrivateGet = privateGet;
            PrivateSet = privateSet;
            Type = type;
        }
    }
}