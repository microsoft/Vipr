// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using ViprUnitTests;

namespace Microsoft.Its.Recipes
{
    internal static partial class Any
    {
        public static IDictionary<string, string> FileAndContentsDictionary(int minElements = 5, params string[] requiredKeys)
        {
            var keys = requiredKeys.Concat(Any.Sequence(i => Any.AlphanumericString(1), Math.Max(0, minElements - requiredKeys.Count())));

            return keys.ToDictionary(k => k, k => Any.String());
        }

        public static TestSettings TestSettings()
        {
            return new TestSettings
            {
                BoolValue = Any.Bool(),
                StringDictionary = Any.StringDictionary(),
                StringValue = Any.String()
            };
        }

        private static IDictionary<string, string> StringDictionary()
        {
            return Any.Sequence(x => Any.String()).ToDictionary(x => x, x => Any.String());
        }
    }
}
