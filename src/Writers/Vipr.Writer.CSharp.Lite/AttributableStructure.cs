// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Vipr.Writer.CSharp.Lite
{
    public abstract class AttributableStructure
    {
        public IEnumerable<Attribute> Attributes { get; protected set; }

        protected AttributableStructure()
        {
            Attributes = new List<Attribute>();
        }
    }
}
