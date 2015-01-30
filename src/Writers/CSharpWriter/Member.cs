// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpWriter
{
    public abstract class Member
    {
        public bool IsOverriding { get; protected set; }
        public bool IsPublic { get; protected set; }
        public bool IsStatic { get; protected set; }

        protected Member()
        {
            IsOverriding = false;
            IsPublic = true;
            IsStatic = false;
        }
    }
}
