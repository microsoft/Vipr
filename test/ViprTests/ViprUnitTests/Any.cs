// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Its.Recipes
{
    internal static partial class Any
    {
        public static IDictionary<string, string> FileAndContentsDictionary(int minElements = 5, params string[] requiredKeys)
        {
            var keys = requiredKeys.Concat(Any.Sequence(i => Any.Word(), Math.Max(0, minElements - requiredKeys.Count())));

            return keys.ToDictionary(k => k, k => Any.String());
        }
    }
}
