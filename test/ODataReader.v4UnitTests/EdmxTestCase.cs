// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Its.Recipes;
using System;
using System.Collections.Generic;
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
        }

        private readonly Dictionary<string, TestNode> _testObjectMap = new Dictionary<string, TestNode>();

        public EdmxTestCase()
        {
            _testObjectMap.Add(Keys.Edmx, new TestNode(string.Empty, string.Empty, Any.Csdl.EdmxToSchema(schema =>
            {
                var @namespace = schema.Attribute("Namespace").Value;
                _testObjectMap.Add(Keys.Schema, new TestNode(@namespace, string.Empty, schema));
                schema.Add(Any.Csdl.EntityContainer(entityContainer => _testObjectMap.Add(Keys.EntityContainer, new TestNode(@namespace, entityContainer.Attribute("Name").Value, entityContainer))));
            })));
        }

        public EdmxTestCase AddEntityType(string entityTypeKey, Action<EdmxTestCase, XElement> config = null)
        {
            return AddEntityType(entityTypeKey, false, config);
        }

        public EdmxTestCase AddEntityType(string entityTypeKey, bool noKey, Action<EdmxTestCase, XElement> config = null)
        {
            var schema = _testObjectMap[Keys.Schema];

            schema.Element.Add(Any.Csdl.EntityType(entityType =>
            {
                var testNode = new TestNode(schema.Namespace, entityType.Attribute("Name").Value, entityType);
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
            }));

            return this;
        }

        public EdmxTestCase AddComplexType(string complexTypeKey, Action<EdmxTestCase, XElement> config = null)
        {
            var schema = _testObjectMap[Keys.Schema];

            schema.Element.Add(Any.Csdl.ComplexType(complexType =>
            {
                var testNode = new TestNode(schema.Namespace, complexType.Attribute("Name").Value, complexType);
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
                var testNode = new TestNode(schema.Namespace, enumType.GetAttribute("Name"), enumType);
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
                var testNode = new TestNode(schema.Namespace, typeDefinitionType.GetAttribute("Name"), typeDefinitionType);
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
                var testNode = new TestNode(schema.Namespace, action.GetAttribute("Name"), action);
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

        public EdmxTestCase AddBoundFunction(string functionKey, string boundEntityTypeKey, Action<EdmxTestCase, XElement> config = null)
        {
            var boundEntityType = _testObjectMap[boundEntityTypeKey];
            var schema = _testObjectMap[Keys.Schema];

            schema.Element.Add(Any.Csdl.Function(Any.Csdl.RandomPrimitiveType(), function =>
            {
                var testNode = new TestNode(schema.Namespace, function.GetAttribute("Name"), function);
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
