// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using ODataReader.v4UnitTests;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

using Resources = ODataReader.v4UnitTests.Properties.Resources;

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
                var element = GetElement(Resources.Action_element);

                config?.Invoke(element);

                return element;
            }

            public static XElement Action(string returnType, Action<XElement> config = null)
            {
                return Action(action =>
                {
                    action.Add(ReturnType(returnType));
                    config?.Invoke(action);
                });
            }

            public static XElement ActionImport(Action<XElement> config = null)
            {
                var element = GetElement(Resources.ActionImport_element);

                config?.Invoke(element);

                return element;
            }

            public static XElement ActionImport(string action, Action<XElement> config = null)
            {
                return ActionImport(actionImport =>
                {
                    actionImport.AddAttribute("Action", action);
                    config?.Invoke(actionImport);
                });
            }

            public static XElement ComplexType(Action<XElement> config = null)
            {
                var element = GetElement(Resources.ComplexType_element);

                config?.Invoke(element);

                return element;
            }

            public static XElement ComplexType(string baseType, Action<XElement> config = null)
            {
                return ComplexType(complexType =>
                {
                    complexType.AddAttribute("BaseType", baseType);
                    config?.Invoke(complexType);
                });
            }

            public static XElement DataServices(Action<XElement> config = null)
            {
                var element = XElement.Parse(Resources.DataServices_element);

                config?.Invoke(element);

                return element;
            }

            public static XElement DescriptionAnnotation(string description, Action<XElement> config = null)
            {
                XElement element = GetAnnotationElement("Org.OData.Core.V1.Description");

                element.AddAttribute("String", description);

                config?.Invoke(element);

                return element;
            }

            public static XElement LongDescriptionAnnotation(string longDescription, Action<XElement> config = null)
            {
                XElement element = GetAnnotationElement("Org.OData.Core.V1.LongDescription");

                element.AddAttribute("String", longDescription);

                config?.Invoke(element);

                return element;
            }

            public static XElement BooleanCapabilityAnnotation(bool value, string term, Action<XElement> config = null)
            {
                XElement element = GetAnnotationElement(term);

                element.AddAttribute("Bool", value);

                config?.Invoke(element);

                return element;
            }

            public static XElement StringListCapabilityAnnotation(IEnumerable<string> value, string term, Action<XElement> config = null)
            {
                XElement element = GetAnnotationElement(term);

                element.Add(StringCollection(value));

                config?.Invoke(element);

                return element;
            }

            public static XElement InsertRestrictionAnnotation(bool insertable, IEnumerable<string> navigationPropertyPaths = null, Action<XElement> config = null)
            {
                XElement element = GetAnnotationElement("Org.OData.Capabilities.V1.InsertRestrictions");

                element.Add(Any.Csdl.Record(record =>
                {
                    record.Add(Any.Csdl.PropertyValue("Insertable", propertyVal => propertyVal.AddAttribute("Bool", insertable)));

                    if (navigationPropertyPaths != null)
                    {
                        record.Add(Any.Csdl.PropertyValue("NonInsertableNavigationProperties",
                            propertyVal => propertyVal.Add(NavigationPropertyPathCollection(navigationPropertyPaths))));
                    }
                }));

                config?.Invoke(element);

                return element;
            }

            public static XElement RecordAnnotation(string term, params string[] properties)
            {
                XElement element = GetAnnotationElement(term);

                element.Add(Any.Csdl.Record(record =>
                {
                    foreach (var property in properties)
                    {
                        record.Add(Any.Csdl.PropertyValue(property, propertyVal => propertyVal.Add(GetStringElement(Any.Word()))));
                    }
                }));

                return element;
            }

            public static XElement RecordCollectionAnnotation(string term, int count, IEnumerable<string> properties)
            {
                XElement element = GetAnnotationElement(term);

                XElement collectionElement = GetCollectionElement();

                for (int i = 0; i < count; i++)
                {
                    collectionElement.Add(Any.Csdl.Record(record =>
                    {
                        foreach (var property in properties)
                        {
                            record.Add(Any.Csdl.PropertyValue(property, propertyVal => propertyVal.Add(GetStringElement(Any.Word()))));
                        }
                    }));
                }

                element.Add(collectionElement);

                return element;
            }


            public static XElement CallbackSupportedAnnotation(int count, Action<XElement> config = null)
            {
                XElement element = GetAnnotationElement("Org.OData.Capabilities.V1.CallbackSupported");

                element.Add(Any.Csdl.Record(record =>
                {
                    record.Add(Any.Csdl.PropertyValue("CallbackProtocols", propertyVal => propertyVal.Add(CallbackProtocolCollection(count))));
                }));

                config?.Invoke(element);

                return element;
            }

            public static XElement DeleteRestrictionAnnotation(bool deletable, IEnumerable<string> navigationPropertyPaths = null, Action<XElement> config = null)
            {
                XElement element = GetAnnotationElement("Org.OData.Capabilities.V1.DeleteRestrictions");

                element.Add(Any.Csdl.Record(record =>
                {
                    record.Add(Any.Csdl.PropertyValue("Deletable", propertyVal => propertyVal.AddAttribute("Bool", deletable)));
                    record.Add(Any.Csdl.PropertyValue("NonDeletableNavigationProperties",
                        propertyVal => propertyVal.Add(NavigationPropertyPathCollection(navigationPropertyPaths))));
                }));

                config?.Invoke(element);

                return element;
            }

            public static XElement UpdateRestrictionAnnotation(bool updatable, IEnumerable<string> navigationPropertyPaths, Action<XElement> config = null)
            {
                XElement element = GetAnnotationElement("Org.OData.Capabilities.V1.UpdateRestrictions");

                element.Add(Any.Csdl.Record(record =>
                {
                    record.Add(Any.Csdl.PropertyValue("Updatable", propertyVal => propertyVal.AddAttribute("Bool", updatable)));
                    record.Add(Any.Csdl.PropertyValue("NonUpdatableNavigationProperties",
                        propertyVal => propertyVal.Add(NavigationPropertyPathCollection(navigationPropertyPaths))));
                }));

                config?.Invoke(element);

                return element;
            }

            public static XElement ChangeTrackingAnnotation(bool value, IEnumerable<string> propertyPaths, Action<XElement> config = null)
            {
                XElement element = GetAnnotationElement("Org.OData.Capabilities.V1.ChangeTracking");

                element.Add(Any.Csdl.Record(record =>
                {
                    record.Add(Any.Csdl.PropertyValue("Supported", propertyVal => propertyVal.AddAttribute("Bool", value)));
                    record.Add(Any.Csdl.PropertyValue("FilterableProperties",
                        propertyVal => propertyVal.Add(PropertyPathCollection(propertyPaths))));
                    record.Add(Any.Csdl.PropertyValue("ExpandableProperties",
                        propertyVal => propertyVal.Add(PropertyPathCollection(propertyPaths))));
                }));

                config?.Invoke(element);

                return element;
            }

            public static XElement FilterRestrictionAnnotation(bool value, IEnumerable<string> propertyPaths, Action<XElement> config = null)
            {
                XElement element = GetAnnotationElement("Org.OData.Capabilities.V1.FilterRestrictions");

                element.Add(Any.Csdl.Record(record =>
                {
                    record.Add(Any.Csdl.PropertyValue("Filterable", propertyVal => propertyVal.AddAttribute("Bool", value)));
                    record.Add(Any.Csdl.PropertyValue("RequiresFilter", propertyVal => propertyVal.AddAttribute("Bool", value)));
                    record.Add(Any.Csdl.PropertyValue("NonFilterableProperties",
                        propertyVal => propertyVal.Add(PropertyPathCollection(propertyPaths))));
                    record.Add(Any.Csdl.PropertyValue("RequiredProperties",
                        propertyVal => propertyVal.Add(PropertyPathCollection(propertyPaths))));
                }));

                config?.Invoke(element);

                return element;
            }

            public static XElement CountRestrictionAnnotation(bool value, IEnumerable<string> propertyPaths, IEnumerable<string> navigationPropertyPaths, Action<XElement> config = null)
            {
                XElement element = GetAnnotationElement("Org.OData.Capabilities.V1.CountRestrictions");

                element.Add(Any.Csdl.Record(record =>
                {
                    record.Add(Any.Csdl.PropertyValue("Countable", propertyVal => propertyVal.AddAttribute("Bool", value)));
                    record.Add(Any.Csdl.PropertyValue("NonCountableProperties",
                        propertyVal => propertyVal.Add(PropertyPathCollection(propertyPaths))));
                    record.Add(Any.Csdl.PropertyValue("NonCountableNavigationProperties",
                        propertyVal => propertyVal.Add(NavigationPropertyPathCollection(navigationPropertyPaths))));
                }));

                config?.Invoke(element);

                return element;
            }

            public static XElement NavigationRestrictionAnnotation(string value, IEnumerable<Tuple<string,string>> navigationPropertyPaths = null, Action<XElement> config = null)
            {
                XElement element = GetAnnotationElement("Org.OData.Capabilities.V1.NavigationRestrictions");

                element.Add(Any.Csdl.Record(record =>
                {
                    var enumElement = GetNavigationTypeElement(value);
                    record.Add(Any.Csdl.PropertyValue("Navigability", propertyVal => propertyVal.Add(enumElement)));

                    if (navigationPropertyPaths != null)
                    {
                        record.Add(Any.Csdl.PropertyValue("RestrictedProperties",
                            propertyVal => propertyVal.Add(NavigationTypeCollection(navigationPropertyPaths))));
                    }
                }));

                config?.Invoke(element);

                return element;
            }

            private static XElement NavigationTypeCollection(IEnumerable<Tuple<string, string>> navigationPropertyPaths)
            {
                XElement collectionElement = GetCollectionElement();

                foreach (var pair in navigationPropertyPaths)
                {
                    collectionElement.Add(Any.Csdl.Record(record =>
                    {
                        record.Add(Any.Csdl.PropertyValue("Navigability", propertyVal => propertyVal.Add(GetNavigationTypeElement(pair.Item2))));

                        XElement navigationPropertyPathElement = XElement.Parse(Resources.NavigationPropertyPath_element);
                        navigationPropertyPathElement.Add(pair.Item1);

                        record.Add(Any.Csdl.PropertyValue("NavigationProperty", propertyVal => propertyVal.Add(navigationPropertyPathElement)));
                    }));
                }

                return collectionElement;
            }

            public static XElement ExpandRestrictionAnnotation(bool expandable, IEnumerable<string> navigationPropertyPaths, Action<XElement> config = null)
            {
                XElement element = GetAnnotationElement("Org.OData.Capabilities.V1.ExpandRestrictions");

                element.Add(Any.Csdl.Record(record =>
                {
                    record.Add(Any.Csdl.PropertyValue("Expandable", propertyVal => propertyVal.AddAttribute("Bool", expandable)));
                    record.Add(Any.Csdl.PropertyValue("NonExpandableProperties",
                        propertyVal => propertyVal.Add(NavigationPropertyPathCollection(navigationPropertyPaths))));
                }));

                config?.Invoke(element);

                return element;
            }
            private static XElement StringCollection(IEnumerable<string> values)
            {
                XElement collectionElement = GetCollectionElement();

                foreach (var value in values)
                {
                    collectionElement.Add(GetStringElement(value));
                }

                return collectionElement;
            }

            private static XElement GetStringElement(string value)
            {
                string stringMember = $"<String xmlns=\"http://docs.oasis-open.org/odata/ns/edm\">{value}</String>";

                return XElement.Parse(stringMember);
            }

            private static XElement GetNavigationTypeElement(string value)
            {
                string enumMember = $"<EnumMember xmlns=\"http://docs.oasis-open.org/odata/ns/edm\">Org.OData.Capabilities.V1.NavigationType/{value}</EnumMember>";

                return XElement.Parse(enumMember);
            }

            private static XElement CallbackProtocolCollection(int count)
            {
                XElement collectionElement = GetCollectionElement();

                for (int i = 0; i < count; i++)
                {
                    collectionElement.Add(Any.Csdl.Record(record =>
                    {
                        record.Add(Any.Csdl.PropertyValue("Id", propertyVal => propertyVal.Add(GetStringElement(Any.Word()))));
                        record.Add(Any.Csdl.PropertyValue("UrlTemplate", propertyVal => propertyVal.Add(GetStringElement(Any.Word()))));
                        record.Add(Any.Csdl.PropertyValue("DocumentationUrl", propertyVal => propertyVal.Add(GetStringElement(Any.Word()))));
                    }
                    ));
                }

                return collectionElement;
            }

            private static XElement NavigationPropertyPathCollection(IEnumerable<string> navigationPropertyPaths)
            {
                XElement collectionElement = GetCollectionElement();

                if (navigationPropertyPaths == null)
                    return collectionElement;

                foreach (var navigationPropertyPath in navigationPropertyPaths)
                {
                    XElement navigationPropertyPathElement = XElement.Parse(Resources.NavigationPropertyPath_element);
                    navigationPropertyPathElement.Add(navigationPropertyPath);
                    collectionElement.Add(navigationPropertyPathElement);
                }

                return collectionElement;
            }

            private static XElement PropertyPathCollection(IEnumerable<string> propertyPaths)
            {
                XElement collectionElement = GetCollectionElement();

                if (propertyPaths == null)
                    return collectionElement;

                foreach (var propertyPath in propertyPaths)
                {
                    XElement propertyPathElement = XElement.Parse(Resources.PropertyPath_element);
                    propertyPathElement.Add(propertyPath);
                    collectionElement.Add(propertyPathElement);
                }

                return collectionElement;
            }

            private static XElement GetAnnotationElement(string term)
            {
                string annotation = string.Format(Resources.Annotation_element, term);

                return XElement.Parse(annotation);
            }

            private static XElement GetCollectionElement()
            {
                return XElement.Parse(Resources.Collection_element);
            }

            public static XElement Edmx(Action<XElement> config = null)
            {
                XElement element = XElement.Parse(Resources.Edmx_element);

                config?.Invoke(element);

                return element;
            }

            public static XElement EntityContainer(Action<XElement> config = null)
            {
                var element = GetElement(Resources.EntityContainer_element);

                config?.Invoke(element);

                return element;
            }

            public static XElement EntitySet(Action<XElement> config = null)
            {
                var element = GetElement(Resources.EntitySet_element);

                config?.Invoke(element);

                return element;
            }

            public static XElement EntityType(Action<XElement> config = null)
            {
                var element = GetElement(Resources.EntityType_element);

                config?.Invoke(element);

                return element;
            }

            public static XElement EntityType(string baseType, Action<XElement> config = null)
            {
                return EntityType(entityType =>
                {
                    entityType.AddAttribute("BaseType", baseType);
                    config?.Invoke(entityType);
                });
            }

            public static XElement EnumType(Action<XElement> config = null)
            {
                var element = GetElement(Resources.EnumType_element);

                config?.Invoke(element);

                return element;
            }

            public static XElement TypeDefinitionType(Action<XElement> config = null)
            {
                var element = GetElement(Resources.TypeDefintionType_element);

                config?.Invoke(element);

                return element;
            }

            public static XElement Function(Action<XElement> config = null)
            {
                string pascalCaseName = PascalCaseName(Int(1, 3));
                string functionString = string.Format(Resources.Function_element, pascalCaseName);

                XElement element = XElement.Parse(functionString);

                config?.Invoke(element);

                return element;
            }

            public static XElement Function(string returnType, Action<XElement> config = null)
            {
                return Function(function =>
                {
                    function.Add(ReturnType(returnType));
                    config?.Invoke(function);
                });
            }
            public static XElement FunctionImport(Action<XElement> config = null)
            {
                var element = GetElement(Resources.FunctionImport_element);

                config?.Invoke(element);

                return element;
            }

            public static XElement Key(Action<XElement> config = null)
            {
                XElement element = XElement.Parse(Resources.Key_element);

                config?.Invoke(element);

                return element;
            }

            public static XElement Member(Action<XElement> config = null)
            {
                var element = GetElement(Resources.Member_element);

                config?.Invoke(element);

                return element;
            }

            public static XElement NavigationProperty(Action<XElement> config = null)
            {
                var element = GetElement(Resources.NavigationProperty_element);

                config?.Invoke(element);

                return element;
            }

            public static XElement NavigationProperty(string type, Action<XElement> config = null)
            {
                var element = GetElement(Resources.NavigationProperty_element);

                element.AddAttribute("Type", type);

                config?.Invoke(element);

                return element;
            }

            public static XElement Parameter(Action<XElement> config = null)
            {
                var element = GetElement(Resources.Parameter_element);

                config?.Invoke(element);

                return element;
            }

            public static XElement Parameter(string type, Action<XElement> config = null)
            {
                return Parameter(parameter =>
                {
                    parameter.AddAttribute("Type", type);
                    config?.Invoke(parameter);
                });
            }

            public static XElement Property(Action<XElement> config = null)
            {
                var element = GetElement(Resources.Property_element);

                config?.Invoke(element);

                return element;
            }

            public static XElement Property(string type, Action<XElement> config = null)
            {
                return Property(property =>
                {
                    property.AddAttribute("Type", type);
                    config?.Invoke(property);
                });
            }

            public static XElement Property(string type, bool isNullable, Action<XElement> config = null)
            {
                return Property(property =>
                {
                    property.AddAttribute("Type", type);
                    property.AddAttribute("Nullable", isNullable);
                    config?.Invoke(property);
                });
            }

            public static XElement PropertyRef(Action<XElement> config = null)
            {
                XElement element = XElement.Parse(Resources.PropertyRef_element);

                config?.Invoke(element);

                return element;
            }

            public static XElement PropertyRef(string name, Action<XElement> config = null)
            {
                return PropertyRef(propertyRef =>
                {
                    propertyRef.AddAttribute("Name", name);
                    config?.Invoke(propertyRef);
                });
            }

            public static XElement PropertyValue(string propertyName, Action<XElement> config = null)
            {
                string propertyString = string.Format(Resources.PropertyValue_element, propertyName);

                XElement element = XElement.Parse(propertyString);

                config?.Invoke(element);

                return element;
            }

            public static XElement Record(Action<XElement> config = null)
            {
                XElement element = XElement.Parse(Resources.Record_element);

                config?.Invoke(element);

                return element;
            }

            public static XElement ReturnType(Action<XElement> config = null)
            {
                XElement element = XElement.Parse(Resources.ReturnType_element);

                config?.Invoke(element);

                return element;
            }

            public static XElement ReturnType(string type, Action<XElement> config = null)
            {
                return ReturnType(returnType =>
                {
                    returnType.AddAttribute("Type", type);
                    config?.Invoke(returnType);
                });
            }

            public static XElement Schema(Action<XElement> config = null)
            {
                string pascalCaseName = PascalCaseName(Int(1, 3));
                var element = GetElement(Resources.Schema_element);

                config?.Invoke(element);

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
                var element = GetElement(Resources.Singleton_element);

                config?.Invoke(element);

                return element;
            }

            private static XElement GetElement(string elementFormat)
            {
                string pascalCaseName = PascalCaseName(Int(1, 3));
                string elementString = string.Format(elementFormat, pascalCaseName);

                return XElement.Parse(elementString);
            }
        }
    }
}