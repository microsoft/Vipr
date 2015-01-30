// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpWriterUnitTests
{
    public static class CodeSamples
    {
        private const string NamespaceTemplate = "namespace {1} {{{0} {2} }}";
        private const string UsingTemplate = "using {0};";
        private const string ClassTemplate = "public class {0} {{{1}}}";

        private static string GetSource(IEnumerable<string> dependencies, string @namespace, string implementation)
        {
            var usings = dependencies.Aggregate(String.Empty, (d, ds) => d + String.Format(UsingTemplate, ds));

            return String.Format(NamespaceTemplate, usings, @namespace, implementation);
        }

        private static string GetClassSource(string className, string implementation)
        {
            return String.Format(ClassTemplate, className, implementation);
        }

        public static string GetStringSource(string @namespace, string type, string retVal)
        {
            var dependencies = new string[] { };

            return GetSource(dependencies, @namespace, GetClassSource(type, String.Format("public string GetString(){{return \"{0}\";}}", retVal)));
        }
    }
}
