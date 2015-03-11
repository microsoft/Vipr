// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CSharpWriter
{
    public abstract class Member
    {
        public bool IsOverriding { get; protected set; }
        public Visibility Visibility { get; protected set; }
        public bool IsStatic { get; protected set; }

        protected Member()
        {
            IsOverriding = false;
            Visibility = Visibility.Public;
            IsStatic = false;
        }
    }
}
