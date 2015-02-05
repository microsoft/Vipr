// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Xml.Linq;
using ODataReader.v4UnitTests;

namespace Microsoft.Its.Recipes
{
    internal partial class Any
    {
        /// <summary>
        /// Generate a CamelCaseName using <see cref="Words"/>
        /// </summary>
        /// <param name="wordCount">Number of words to compose together as the name</param>
        public static string PascalCaseName(int wordCount = 4)
        {
            string camelCaseName = CamelCaseName(wordCount);
            return camelCaseName.Substring(0, 1).ToLowerInvariant() + camelCaseName.Substring(1);
        }


        internal static class Csdl
        {
            private readonly static string[] PrimitiveTypes = new[]
            {
                "Edm.Binary",
                "Edm.Boolean",
                "Edm.Byte",
                "Edm.Date",
                "Edm.DateTimeOffset",
                "Edm.Decimal",
                "Edm.Double",
                "Edm.Duration",
                "Edm.Guid",
                "Edm.Int16",
                "Edm.Int32",
                "Edm.Int64",
                "Edm.SByte",
                "Edm.Single",
                "Edm.Stream",
                "Edm.String",
                "Edm.TimeOfDay",
                "Edm.Geography",
                "Edm.GeographyPoint",
                "Edm.GeographyLineString",
                "Edm.GeographyPolygon",
                "Edm.GeographyMultiPoint",
                "Edm.GeographyMultiLineString",
                "Edm.GeographyMultiPolygon",
                "Edm.GeographyCollection",
                "Edm.Geometry",
                "Edm.GeometryPoint",
                "Edm.GeometryLineString",
                "Edm.GeometryPolygon",
                "Edm.GeometryMultiPoint",
                "Edm.GeometryMultiLineString",
                "Edm.GeometryMultiPolygon",
                "Edm.GeometryCollection"
            };

            public static string RandomPrimitiveType()
            {
                return PrimitiveTypes.RandomElement();
            }
            public static string DefaultEnumUnderlyingType()
            {
                return PrimitiveTypes[10];
            }

            public static string RandomEnumUnderlyingType()
            {
                int[] primitiveTypeIndices = {2, 9, 10, 11, 12};
                return PrimitiveTypes[primitiveTypeIndices.RandomElement()];
            }

            public static XElement Action(Action<XElement> config = null)
            {
                var pascalCaseName = PascalCaseName(Int(1, 3));
                var actionString = string.Format(ODataReader.v4UnitTests.Properties.Resources.Action_element, pascalCaseName);

                var element = XElement.Parse(actionString);

                if (config != null) config(element);

                return element;
            }

            public static XElement ActionImport(Action<XElement> config = null)
            {
                string pascalCaseName = PascalCaseName(Int(1, 3));
                string actionImportString =
                    string.Format(ODataReader.v4UnitTests.Properties.Resources.ActionImport_element, pascalCaseName);

                XElement element = XElement.Parse(actionImportString);

                if (config != null) config(element);

                return element;
            }

            public static XElement ComplexType(Action<XElement> config = null)
            {
                string pascalCaseName = PascalCaseName(Int(1, 3));
                string complexTypeString =
                    string.Format(ODataReader.v4UnitTests.Properties.Resources.ComplexType_element, pascalCaseName);

                XElement element = XElement.Parse(complexTypeString);

                if (config != null) config(element);

                return element;
            }

            public static XElement DataServices(Action<XElement> config = null)
            {
                var element = XElement.Parse(ODataReader.v4UnitTests.Properties.Resources.DataServices_element);

                if (config != null) config(element);

                return element;
            }

            public static XElement Edmx(Action<XElement> config = null)
            {
                XElement element = XElement.Parse(ODataReader.v4UnitTests.Properties.Resources.Edmx_element);

                if (config != null) config(element);

                return element;
            }

            public static XElement EntityContainer(Action<XElement> config = null)
            {
                string pascalCaseName = PascalCaseName(Int(1, 3));
                string entityContainerString =
                    string.Format(ODataReader.v4UnitTests.Properties.Resources.EntityContainer_element, pascalCaseName);

                XElement element = XElement.Parse(entityContainerString);

                if (config != null) config(element);

                return element;
            }

            public static XElement EntitySet(Action<XElement> config = null)
            {
                string pascalCaseName = PascalCaseName(Int(1, 3));
                string entitySetString =
                    string.Format(ODataReader.v4UnitTests.Properties.Resources.EntitySet_element, pascalCaseName);

                XElement element = XElement.Parse(entitySetString);

                if (config != null) config(element);

                return element;
            }

            public static XElement EntityType(Action<XElement> config = null)
            {
                string pascalCaseName = PascalCaseName(Int(1, 3));
                string entityTypeString =
                    string.Format(ODataReader.v4UnitTests.Properties.Resources.EntityType_element, pascalCaseName);

                XElement element = XElement.Parse(entityTypeString);

                if (config != null) config(element);

                return element;
            }

            public static XElement EnumType(Action<XElement> config = null)
            {
                string pascalCaseName = PascalCaseName(Int(1, 3));
                string enumTypeString = string.Format(ODataReader.v4UnitTests.Properties.Resources.EnumType_element,
                    pascalCaseName);

                XElement element = XElement.Parse(enumTypeString);

                if (config != null) config(element);

                return element;
            }

            public static XElement Function(Action<XElement> config = null)
            {
                string pascalCaseName = PascalCaseName(Int(1, 3));
                string functionString =
                    string.Format(ODataReader.v4UnitTests.Properties.Resources.Function_element, pascalCaseName);

                XElement element = XElement.Parse(functionString);

                if (config != null) config(element);

                return element;
            }

            public static XElement FunctionImport(Action<XElement> config = null)
            {
                string pascalCaseName = PascalCaseName(Int(1, 3));
                string functionImportString =
                    string.Format(ODataReader.v4UnitTests.Properties.Resources.FunctionImport_element, pascalCaseName);

                XElement element = XElement.Parse(functionImportString);

                if (config != null) config(element);

                return element;
            }

            public static XElement Key(Action<XElement> config = null)
            {
                XElement element = XElement.Parse(ODataReader.v4UnitTests.Properties.Resources.Key_element);

                if (config != null) config(element);

                return element;
            }

            public static XElement Member(Action<XElement> config = null)
            {
                string pascalCaseName = PascalCaseName(Int(1, 3));
                string memberString =
                    string.Format(ODataReader.v4UnitTests.Properties.Resources.Member_element, pascalCaseName);

                XElement element = XElement.Parse(memberString);

                if (config != null) config(element);

                return element;
            }

            public static XElement NavigationProperty(Action<XElement> config = null)
            {
                string pascalCaseName = PascalCaseName(Int(1, 3));
                string navigationPropertyString =
                    string.Format(ODataReader.v4UnitTests.Properties.Resources.NavigationProperty_element,
                        pascalCaseName);

                XElement element = XElement.Parse(navigationPropertyString);

                if (config != null) config(element);

                return element;
            }

            public static XElement Parameter(Action<XElement> config = null)
            {
                string pascalCaseName = PascalCaseName(Int(1, 3));
                string parameterString =
                    string.Format(ODataReader.v4UnitTests.Properties.Resources.Parameter_element, pascalCaseName);

                XElement element = XElement.Parse(parameterString);

                if (config != null) config(element);

                return element;
            }

            public static XElement Property(Action<XElement> config = null)
            {
                string pascalCaseName = PascalCaseName(Int(1, 3));
                string propertyString =
                    string.Format(ODataReader.v4UnitTests.Properties.Resources.Property_element, pascalCaseName);

                XElement element = XElement.Parse(propertyString);

                if (config != null) config(element);

                return element;
            }

            public static XElement Property(string type, Action<XElement> config = null)
            {
                return Property(property =>
                {
                    property.AddAttribute("Type", type);
                    if (config != null) config(property);
                });
            }

            public static XElement PropertyRef(Action<XElement> config = null)
            {
                XElement element = XElement.Parse(ODataReader.v4UnitTests.Properties.Resources.PropertyRef_element);

                if (config != null) config(element);

                return element;
            }

            public static XElement PropertyRef(string name, Action<XElement> config = null)
            {
                return PropertyRef(propertyRef =>
                {
                    propertyRef.AddAttribute("Name", name);
                    if(config != null) config(propertyRef);
                });
            }

            public static XElement ReturnType(Action<XElement> config = null)
            {
                string pascalCaseName = PascalCaseName(Int(1, 3));
                string returnTypeString =
                    string.Format(ODataReader.v4UnitTests.Properties.Resources.ReturnType_element, pascalCaseName);

                XElement element = XElement.Parse(returnTypeString);

                if (config != null) config(element);

                return element;
            }

            public static XElement Schema(Action<XElement> config = null)
            {
                string pascalCaseName = PascalCaseName(Int(1, 3));
                string schemaString = string.Format(ODataReader.v4UnitTests.Properties.Resources.Schema_element,
                    pascalCaseName);

                XElement element = XElement.Parse(schemaString);

                if (config != null) config(element);

                return element;
            }

            public static XElement EdmxToSchema(Action<XElement> config = null)
            {
                return Edmx(edmx => 
                    edmx.Add(DataServices(dataServices => 
                        dataServices.Add(Schema(config)))));
            }

            public static XElement Singleton(Action<XElement> config = null)
            {
                string pascalCaseName = PascalCaseName(Int(1, 3));
                string singletonString =
                    string.Format(ODataReader.v4UnitTests.Properties.Resources.Singleton_element, pascalCaseName);

                XElement element = XElement.Parse(singletonString);

                if (config != null) config(element);

                return element;
            }
        }
    }
}