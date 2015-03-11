// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml.Linq;

namespace ODataReader.v4UnitTests
{
    public static class XElementExtensions
    {
        public static void AddAttribute(this XElement element, string name, object value)
        {
            element.Add(new XAttribute(name, value));
        }

        public static string GetAttribute(this XElement element, string name)
        {
            return element.Attribute(name).Value;
        }
    }
}
