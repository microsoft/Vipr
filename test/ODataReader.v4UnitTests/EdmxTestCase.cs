// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Its.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Vipr.Core;

namespace ODataReader.v4UnitTests
{
    public class EdmxTestCase
    {
        public class TestNode
        {
            public string Namespace;
            public string Name;
            public XElement Element;

            public TestNode(string @namespace, string name, XElement element)
            {
                Namespace = @namespace;
                Name = name;
                Element = element;
            }

            public TestNode(string @namespace, XElement element)
            {
                Namespace = @namespace;
                Element = element;

                var attrName = element.Attribute("Name");
                Name = attrName == null ? string.Empty : attrName.Value;
            }

            public string FullName()
            {
                return Namespace + "." + Name;
            }
        }

        public class Keys
        {
            public const string Action = "$action";
            public const string ComplexType = "$complexType";
            public const string ComplexTypeBase = "$complexType#Base";
            public const string Edmx = "$edmx";
            public const string EntityContainer = "$entityContainer";
            public const string EntityType = "$entityType";
            public const string EntityTypeBase = "$entityType#Base";
            public const string EnumType = "$enumType";
            public const string TypeDefinitionType = "$typeDefinitionType";
            public const string Function = "$function";
            public const string Schema = "$schema";
            public const string DataServices = "$dataServices";
        }

        private readonly Dictionary<string, TestNode> _testObjectMap = new Dictionary<string, TestNode>();

        public EdmxTestCase()
        {
            var edmx = Any.Csdl.EdmxToSchema(element => element.Add(Any.Csdl.EntityContainer()));

            var dataServices = edmx.Descendants().Single(x => x.Name.LocalName == "DataServices");
            var schema = dataServices.Descendants().Single(x => x.Name.LocalName == "Schema");
            var entityContainer = schema.Descendants().Single(x => x.Name.LocalName == "EntityContainer");

            var @namespace = schema.Attribute("Namespace").Value;

            _testObjectMap[Keys.Edmx] = new TestNode(string.Empty, string.Empty, edmx);
            _testObjectMap[Keys.DataServices] = new TestNode(string.Empty, string.Empty, dataServices);
            _testObjectMap[Keys.Schema] = new TestNode(@namespace, string.Empty, schema);
            _testObjectMap[Keys.EntityContainer] = new TestNode(@namespace, entityContainer);
        }

        public EdmxTestCase AddEntityType(string entityTypeKey, Action<EdmxTestCase, XElement> config = null)
        {
            return AddEntityType(entityTypeKey, false, config);
        }

        public EdmxTestCase AddEntityType(string entityTypeKey, bool noKey, Action<EdmxTestCase, XElement> config = null)
        {
            _testObjectMap[Keys.Schema].Element.Add(CreateEntityType(entityTypeKey, noKey, config));
            return this;
        }

        public EdmxTestCase AddEntityType(XElement entityTypeElement)
        {
            _testObjectMap[Keys.Schema].Element.Add(entityTypeElement);
            return this;
        }

        public XElement CreateEntityType(string entityTypeKey, bool noKey, Action<EdmxTestCase, XElement> config = null)
        {
            var schema = _testObjectMap[Keys.Schema];

            return Any.Csdl.EntityType(entityType =>
            {
                var testNode = new TestNode(schema.Namespace, entityType);
                _testObjectMap.Add(entityTypeKey, testNode);

                if (!noKey)
                {
                    entityType.Add(Any.Csdl.Key(key =>
                    {
                        foreach (
                            var property in
                                Any.Sequence(
                                    i => Any.Csdl.Property(Any.Csdl.RandomPrimitiveType(), false),
                                    Any.Int(1, 2)))
                        {
                            entityType.Add(property);
                            key.Add(Any.Csdl.PropertyRef(property.Attribute("Name").Value));
                        }
                    }));
                }

                foreach (
                    var property in
                        Any.Sequence(
                            i => Any.Csdl.Property(Any.Csdl.RandomPrimitiveType()),
                            Any.Int(1, 3)))
                {
                    entityType.Add(property);
                }

                if (config != null)
                {
                    config(this, entityType);
                }
            });
        }

        public EdmxTestCase AddComplexType(string complexTypeKey, Action<EdmxTestCase, XElement> config = null)
        {
            var schema = _testObjectMap[Keys.Schema];

            schema.Element.Add(Any.Csdl.ComplexType(complexType =>
            {
                var testNode = new TestNode(schema.Namespace, complexType);
                _testObjectMap.Add(complexTypeKey, testNode);

                foreach (
                    var property in
                        Any.Sequence(
                            i =>
                                Any.Csdl.Property(
                                    property => property.AddAttribute("Type", Any.Csdl.RandomPrimitiveType())),
                            Any.Int(1, 5)))
                {
                    complexType.Add(property);
                }

                if (config != null)
                {
                    config(this, complexType);
                }
            }));

            return this;
        }

        public EdmxTestCase AddEnumType(string enumTypeKey, Action<EdmxTestCase, XElement> config = null)
        {
            var schema = _testObjectMap[Keys.Schema];

            schema.Element.Add(Any.Csdl.EnumType(enumType =>
            {
                var testNode = new TestNode(schema.Namespace, enumType);
                _testObjectMap.Add(enumTypeKey, testNode);

                foreach (var member in Any.Sequence((i) => Any.Csdl.Member(), Any.Int(1, 5)))
                {
                    enumType.Add(member);
                }

                if (config != null)
                {
                    config(this, enumType);
                }
            }));

            return this;
        }

        public EdmxTestCase AddTypeDefinitionType(string typeDefinitionTypeKey, Action<EdmxTestCase, XElement> config = null)
        {
            var schema = _testObjectMap[Keys.Schema];

            schema.Element.Add(Any.Csdl.TypeDefinitionType(typeDefinitionType =>
            {
                var testNode = new TestNode(schema.Namespace, typeDefinitionType);
                _testObjectMap.Add(typeDefinitionTypeKey, testNode);

                if (config != null)
                {
                    config(this, typeDefinitionType);
                }
            }));

            return this;
        }

        public EdmxTestCase AddBoundAction(string actionKey, string boundEntityTypeKey, Action<EdmxTestCase, XElement> config = null)
        {
            var boundEntityType = _testObjectMap[boundEntityTypeKey];
            var schema = _testObjectMap[Keys.Schema];

            schema.Element.Add(Any.Csdl.Action(Any.Csdl.RandomPrimitiveType(), action =>
            {
                var testNode = new TestNode(schema.Namespace, action);
                _testObjectMap.Add(actionKey, testNode);

                action.AddAttribute("IsBound", true);
                action.Add(Any.Csdl.Parameter(boundEntityType.FullName()));
                foreach (
                    var parameter in
                        Any.Sequence(i => Any.Csdl.Parameter(Any.Csdl.RandomPrimitiveType()), Any.Int(1, 3)))
                {
                    action.Add(parameter);
                }

                if (config != null)
                {
                    config(this, action);
                }
            }));

            return this;
        }

        public EdmxTestCase AddBoundFunction(string functionKey, string boundEntityTypeKey, XElement schemaElement = null, Action<EdmxTestCase, XElement> config = null)
        {
            var boundEntityType = _testObjectMap[boundEntityTypeKey];
            var schema = _testObjectMap[Keys.Schema];

            if (schemaElement != null)
            {
                var functionNamespace = schemaElement.Attribute("Namespace").Value;
                schema = new EdmxTestCase.TestNode(functionNamespace, string.Empty, schemaElement);
            }

            schema.Element.Add(Any.Csdl.Function(Any.Csdl.RandomPrimitiveType(), function =>
            {
                var testNode = new TestNode(schema.Namespace, function);
                _testObjectMap.Add(functionKey, testNode);

                function.AddAttribute("IsBound", true);
                function.Add(Any.Csdl.Parameter(boundEntityType.FullName()));
                foreach (
                    var parameter in
                        Any.Sequence(i => Any.Csdl.Parameter(Any.Csdl.RandomPrimitiveType()), Any.Int(1, 3)))
                {
                    function.Add(parameter);
                }

                if (config != null)
                {
                    config(this, function);
                }
            }));

            if (schemaElement != null)
            {
                var dataServices = _testObjectMap[Keys.DataServices];
                dataServices.Element.Add(schemaElement);
            }

            return this;
        }

        public TestNode this[string key]
        {
            get { return _testObjectMap[key]; }
        }

        public IEnumerable<TextFile> ServiceMetadata()
        {
            yield return new TextFile("$metadata", _testObjectMap[Keys.Edmx].Element.ToString());
        }
    }
}
