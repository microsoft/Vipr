// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
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

        public static string GetName(this XElement element)
        {
            return element.GetAttribute("Name");
        }

        public static string GetAnnotationTerm(this XElement annotation)
        {
            string termName = string.Empty;
            if (annotation.Name.LocalName == "Annotation")
            {
                termName = annotation.GetAttribute("Term");
            }

            return termName;
        }

        public static void SetAnnotation(this XElement element, XElement annotation)
        {
            string term = annotation.GetAnnotationTerm();

            if (!string.IsNullOrEmpty(term))
            {
                var existing = element
                                .Elements()
                                .FirstOrDefault(e => e.GetAnnotationTerm() == term);

                if (existing != null)
                {
                    element.SetElementValue(existing.Name, null);
                }

                element.Add(annotation);
            }
        }
    }
}
