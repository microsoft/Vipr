// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace CSharpWriter
{
    public class ParameterizedFunction : ParameterizedMember
    {
        public Type ReturnType { get; protected set; }
    }
}