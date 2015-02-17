// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace CSharpWriter
{
    internal class Feature
    {
        public string Name { get; set; }
        public IEnumerable<Enum> Enums { get; set; }
        public IEnumerable<Class> Classes { get; set; }
        public IEnumerable<Interface> Interfaces { get; set; }

        internal Feature()
        {
            Enums = Enumerable.Empty<Enum>();
            Classes = Enumerable.Empty<Class>();
            Interfaces = Enumerable.Empty<Interface>();
        }
    }
}