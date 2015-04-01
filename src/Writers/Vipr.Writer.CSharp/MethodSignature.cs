// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpWriter
{
    public abstract class MethodSignature : ParameterizedFunction
    {
        public IEnumerable<String> GenericParameters { get; protected set; }
        public bool IsAsync { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
    }
}
