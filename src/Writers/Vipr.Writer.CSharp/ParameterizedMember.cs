// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace CSharpWriter
{
    public class ParameterizedMember : ExplicitlyImplementableMember
    {
        public IEnumerable<Parameter> Parameters { get; protected set; }
    }
}