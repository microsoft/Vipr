// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using ODataReader.v4UnitTests;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

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
            private readonly static string[] PrimitiveTypes =
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

            public static XElement Action(string returnType, Action<XElement> config = null)
            {
                return Action(action =>
                {
                    action.Add(ReturnType(returnType));
                    if (config != null) config(action);
                });
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

            public static XElement ActionImport(string action, Action<XElement> config = null)
            {
                return ActionImport(actionImport =>
                {
                    actionImport.AddAttribute("Action", action);
                    if (config != null) config(actionImport);
                });
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

            public static XElement ComplexType(string baseType, Action<XElement> config = null)
            {
                return ComplexType(complexType =>
                {
                    complexType.AddAttribute("BaseType", baseType);
                    if (config != null) config(complexType);
                });
            }

            public static XElement DataServices(Action<XElement> config = null)
            {
                var element = XElement.Parse(ODataReader.v4UnitTests.Properties.Resources.DataServices_element);

                if (config != null) config(element);

                return element;
            }

            public static XElement DescriptionAnnotation(string description, Action<XElement> config = null)
            {
                string descriptionAnnotationString =
                    string.Format(ODataReader.v4UnitTests.Properties.Resources.Annotation_element, "Org.OData.Core.V1.Description");

                XElement element = XElement.Parse(descriptionAnnotationString);
                element.AddAttribute("String", description);

                if (config != null) config(element);

                return element;
            }

            public static XElement LongDescriptionAnnotation(string longDescription, Action<XElement> config = null)
            {
                string longDescriptionAnnotationString =
                    string.Format(ODataReader.v4UnitTests.Properties.Resources.Annotation_element, "Org.OData.Core.V1.LongDescription");

                XElement element = XElement.Parse(longDescriptionAnnotationString);
                element.AddAttribute("String", longDescription);

                if (config != null) config(element);

                return element;
            }

            public static XElement InsertRestrictionAnnotation(bool insertable, IEnumerable<string> navigationPropertyPaths, Action<XElement> config = null)
            {
                string insertAnnotation =
                    string.Format(ODataReader.v4UnitTests.Properties.Resources.Annotation_element, "Org.OData.Capabilities.V1.InsertRestrictions");

                XElement element = XElement.Parse(insertAnnotation);

                element.Add(Any.Csdl.Record(record =>
                {
                    record.Add(Any.Csdl.PropertyValue("Insertable", propertyVal => propertyVal.AddAttribute("Bool", insertable)));
                    record.Add(Any.Csdl.PropertyValue("NonInsertableNavigationProperties",
                        propertyVal => propertyVal.Add(NavigationPropertyPathCollection(navigationPropertyPaths))));
                }));

                if (config != null) config(element);

                return element;
            }

            public static XElement DeleteRestrictionAnnotation(bool deletable, IEnumerable<string> navigationPropertyPaths, Action<XElement> config = null)
            {
                string deleteAnnotation =
                    string.Format(ODataReader.v4UnitTests.Properties.Resources.Annotation_element, "Org.OData.Capabilities.V1.DeleteRestrictions");

                XElement element = XElement.Parse(deleteAnnotation);

                element.Add(Any.Csdl.Record(record =>
                {
                    record.Add(Any.Csdl.PropertyValue("Deletable", propertyVal => propertyVal.AddAttribute("Bool", deletable)));
                    record.Add(Any.Csdl.PropertyValue("NonDeletableNavigationProperties",
                        propertyVal => propertyVal.Add(NavigationPropertyPathCollection(navigationPropertyPaths))));
                }));

                if (config != null) config(element);

                return element;
            }

            public static XElement UpdateRestrictionAnnotation(bool updatable, IEnumerable<string> navigationPropertyPaths, Action<XElement> config = null)
            {
                string updateAnnotation =
                    string.Format(ODataReader.v4UnitTests.Properties.Resources.Annotation_element, "Org.OData.Capabilities.V1.UpdateRestrictions");

                XElement element = XElement.Parse(updateAnnotation);

                element.Add(Any.Csdl.Record(record =>
                {
                    record.Add(Any.Csdl.PropertyValue("Updatable", propertyVal => propertyVal.AddAttribute("Bool", updatable)));
                    record.Add(Any.Csdl.PropertyValue("NonUpdatableNavigationProperties",
                        propertyVal => propertyVal.Add(NavigationPropertyPathCollection(navigationPropertyPaths))));
                }));

                if (config != null) config(element);

                return element;
            }

            public static XElement ExpandRestrictionAnnotation(bool expandable, IEnumerable<string> navigationPropertyPaths, Action<XElement> config = null)
            {
                string expandAnnotation =
                    string.Format(ODataReader.v4UnitTests.Properties.Resources.Annotation_element, "Org.OData.Capabilities.V1.ExpandRestrictions");

                XElement element = XElement.Parse(expandAnnotation);

                element.Add(Any.Csdl.Record(record =>
                {
                    record.Add(Any.Csdl.PropertyValue("Expandable", propertyVal => propertyVal.AddAttribute("Bool", expandable)));
                    record.Add(Any.Csdl.PropertyValue("NonExpandableProperties",
                        propertyVal => propertyVal.Add(NavigationPropertyPathCollection(navigationPropertyPaths))));
                }));

                if (config != null) config(element);

                return element;
            }

            private static XElement NavigationPropertyPathCollection(IEnumerable<string> navigationPropertyPaths)
            {
                XElement collectionElement = XElement.Parse(ODataReader.v4UnitTests.Properties.Resources.Collection_element);

                if (navigationPropertyPaths == null)
                    return collectionElement;

                foreach (var navigationPropertyPath in navigationPropertyPaths)
                {
                    XElement navigationPropertyPathElement = XElement.Parse(ODataReader.v4UnitTests.Properties.Resources.NavigationPropertyPath_element);
                    navigationPropertyPathElement.Add(navigationPropertyPath);
                    collectionElement.Add(navigationPropertyPathElement);
                }

                return collectionElement;
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

            public static XElement EntityType(string baseType, Action<XElement> config = null)
            {
                return EntityType(entityType =>
                {
                    entityType.AddAttribute("BaseType", baseType);
                    if (config != null) config(entityType);
                });
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

            public static XElement Function(string returnType, Action<XElement> config = null)
            {
                return Function(function =>
                {
                    function.Add(ReturnType(returnType));
                    if (config != null) config(function);
                });
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

            public static XElement NavigationProperty(string type, Action<XElement> config = null)
            {
                string pascalCaseName = PascalCaseName(Int(1, 3));
                string navigationPropertyString =
                    string.Format(ODataReader.v4UnitTests.Properties.Resources.NavigationProperty_element,
                        pascalCaseName);

                XElement element = XElement.Parse(navigationPropertyString);

                element.AddAttribute("Type", type);

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

            public static XElement Parameter(string type, Action<XElement> config = null)
            {
                return Parameter(parameter =>
                {
                    parameter.AddAttribute("Type", type);
                    if (config != null) config(parameter);
                });
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

            public static XElement Property(string type, bool isNullable, Action<XElement> config = null)
            {
                return Property(property =>
                {
                    property.AddAttribute("Type", type);
                    property.AddAttribute("Nullable", isNullable);
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

            public static XElement PropertyValue(string propertyName, Action<XElement> config = null)
            {
                string propertyString =
                    string.Format(ODataReader.v4UnitTests.Properties.Resources.PropertyValue_element, propertyName);

                XElement element = XElement.Parse(propertyString);

                if (config != null) config(element);

                return element;
            }

            public static XElement Record(Action<XElement> config = null)
            {
                XElement element = XElement.Parse(ODataReader.v4UnitTests.Properties.Resources.Record_element);

                if (config != null) config(element);

                return element;
            }

            public static XElement ReturnType(Action<XElement> config = null)
            {
                XElement element = XElement.Parse(ODataReader.v4UnitTests.Properties.Resources.ReturnType_element);

                if (config != null) config(element);

                return element;
            }

            public static XElement ReturnType(string type, Action<XElement> config = null)
            {
                return ReturnType(returnType =>
                {
                    returnType.AddAttribute("Type", type);
                    if (config != null) config(returnType);
                });
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