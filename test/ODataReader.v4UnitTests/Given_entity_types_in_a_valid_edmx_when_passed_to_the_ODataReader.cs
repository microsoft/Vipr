// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Its.Recipes;
using ODataReader.v4;
using System.Collections.Generic;
using Vipr.Core.CodeModel;
using Xunit;
using Xunit.Sdk;

namespace ODataReader.v4UnitTests
{
    public class Given_entity_types_in_a_valid_edmx_when_passed_to_the_ODataReader
    {
        private ODataReader.v4.OdcmReader _odcmReader;

        public Given_entity_types_in_a_valid_edmx_when_passed_to_the_ODataReader()
        {
            _odcmReader = new OdcmReader();
        }

        [Fact]
        public void It_returns_one_OdcmEntityClass_for_each_EntityType()
        {
            var entityTypeName = string.Empty;
            var schemaNamespace = string.Empty;
            var propertyCount = 0;
            var keyCount = 0;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schemaNamespace = schema.Attribute("Namespace").Value;
                schema.Add(Any.Csdl.EntityType(entityType =>
                {
                    entityTypeName = entityType.Attribute("Name").Value;
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
                            keyCount++;
                        }
                    }));
                    foreach (
                        var property in
                            Any.Sequence(
                                i => Any.Csdl.Property(Any.Csdl.RandomPrimitiveType()),
                                Any.Int(1, 3)))
                    {
                        entityType.Add(property);
                        propertyCount++;
                    }
                }));
                schema.Add(Any.Csdl.EntityContainer());
            });

            var serviceMetadata = new Dictionary<string, string>()
            {
                {"$metadata", edmxElement.ToString()}
            };
            var odcmModel = _odcmReader.GenerateOdcmModel(serviceMetadata);

            OdcmType odcmEntityType;
            odcmModel.TryResolveType(entityTypeName, schemaNamespace, out odcmEntityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmEntityType
                .Should()
                .BeOfType<OdcmEntityClass>("because entity types should result in an OdcmClass");
            odcmEntityType.As<OdcmEntityClass>().Key.Count
                .Should()
                .Be(keyCount, "because each property reference added to the key of an entity type should result in an OdcmClass property");
            odcmEntityType.As<OdcmEntityClass>().Properties.Count
                .Should()
                .Be(propertyCount + keyCount, "because each property added to an entity type should result in an OdcmClass property");
            odcmEntityType.As<OdcmEntityClass>().IsAbstract
                .Should()
                .BeFalse("because an entity type with no Abstract facet should be false in the OdcmModel");
            odcmEntityType.As<OdcmEntityClass>().IsOpen
                .Should()
                .BeFalse("because an entity type with no OpenType facet should be false in the OdcmModel");
        }

        [Fact]
        public void When_Abstract_is_set_it_returns_an_OdcmEntityClass_with_IsAbstract_set()
        {
            var entityTypeName = string.Empty;
            var schemaNamespace = string.Empty;
            var propertyCount = 0;
            var keyCount = 0;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schemaNamespace = schema.Attribute("Namespace").Value;
                schema.Add(Any.Csdl.EntityType(entityType =>
                {
                    entityTypeName = entityType.Attribute("Name").Value;
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
                            keyCount++;
                        }
                    }));
                    foreach (
                        var property in
                            Any.Sequence(
                                i => Any.Csdl.Property(Any.Csdl.RandomPrimitiveType()),
                                Any.Int(1, 3)))
                    {
                        entityType.Add(property);
                        propertyCount++;
                    }
                    entityType.AddAttribute("Abstract", true);
                }));
                schema.Add(Any.Csdl.EntityContainer());
            });

            var serviceMetadata = new Dictionary<string, string>()
            {
                {"$metadata", edmxElement.ToString()}
            };
            var odcmModel = _odcmReader.GenerateOdcmModel(serviceMetadata);

            OdcmType odcmEntityType;
            odcmModel.TryResolveType(entityTypeName, schemaNamespace, out odcmEntityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmEntityType
                .Should()
                .BeOfType<OdcmEntityClass>("because entity types should result in an OdcmClass");
            odcmEntityType.As<OdcmEntityClass>().IsAbstract
                .Should()
                .BeTrue("because an entity type with the Abstract facet set should be abstract in the OdcmModel");
        }

        [Fact]
        public void When_OpenType_is_set_it_returns_an_OdcmEntityClass_with_IsOpen_set()
        {
            var entityTypeName = string.Empty;
            var schemaNamespace = string.Empty;
            var propertyCount = 0;
            var keyCount = 0;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schemaNamespace = schema.Attribute("Namespace").Value;
                schema.Add(Any.Csdl.EntityType(entityType =>
                {
                    entityTypeName = entityType.Attribute("Name").Value;
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
                            keyCount++;
                        }
                    }));
                    foreach (
                        var property in
                            Any.Sequence(
                                i => Any.Csdl.Property(Any.Csdl.RandomPrimitiveType()),
                                Any.Int(1, 3)))
                    {
                        entityType.Add(property);
                        propertyCount++;
                    }
                    entityType.AddAttribute("OpenType", true);
                }));
                schema.Add(Any.Csdl.EntityContainer());
            });

            var serviceMetadata = new Dictionary<string, string>()
            {
                {"$metadata", edmxElement.ToString()}
            };
            var odcmModel = _odcmReader.GenerateOdcmModel(serviceMetadata);

            OdcmType odcmEntityType;
            odcmModel.TryResolveType(entityTypeName, schemaNamespace, out odcmEntityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmEntityType
                .Should()
                .BeOfType<OdcmEntityClass>("because entity types should result in an OdcmClass");
            odcmEntityType.As<OdcmEntityClass>().IsOpen
                .Should()
                .BeTrue("because an entity type with the OpenType facet set should be open in the OdcmModel");
        }

        [Fact]
        public void When_HasStream_is_set_it_returns_an_OdcmMediaClass()
        {
            var entityTypeName = string.Empty;
            var schemaNamespace = string.Empty;
            var propertyCount = 0;
            var keyCount = 0;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schemaNamespace = schema.Attribute("Namespace").Value;
                schema.Add(Any.Csdl.EntityType(entityType =>
                {
                    entityTypeName = entityType.Attribute("Name").Value;
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
                            keyCount++;
                        }
                    }));
                    foreach (
                        var property in
                            Any.Sequence(
                                i => Any.Csdl.Property(Any.Csdl.RandomPrimitiveType()),
                                Any.Int(1, 3)))
                    {
                        entityType.Add(property);
                        propertyCount++;
                    }
                    entityType.AddAttribute("HasStream", true);
                }));
                schema.Add(Any.Csdl.EntityContainer());
            });

            var serviceMetadata = new Dictionary<string, string>()
            {
                {"$metadata", edmxElement.ToString()}
            };
            var odcmModel = _odcmReader.GenerateOdcmModel(serviceMetadata);

            OdcmType odcmEntityType;
            odcmModel.TryResolveType(entityTypeName, schemaNamespace, out odcmEntityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmEntityType
                .Should()
                .BeOfType<OdcmMediaClass>("because an entity type with the HasStream facet set should be an OdcmMediaClass");
        }

        [Fact]
        public void When_BaseType_is_set_it_returns_an_OdcmEntityClass_with_a_BaseType_set()
        {
            var baseTypeName = string.Empty;
            var entityTypeName = string.Empty;
            var schemaNamespace = string.Empty;
            var propertyCount = 0;
            var keyCount = 0;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schemaNamespace = schema.GetAttribute("Namespace");
                schema.Add(Any.Csdl.EntityType(entityType =>
                {
                    baseTypeName = entityType.GetAttribute("Name");
                    entityType.AddAttribute("Abstract", true);
                    entityType.Add(Any.Csdl.Key(key =>
                    {
                        foreach (
                            var property in
                                Any.Sequence(
                                    i => Any.Csdl.Property(Any.Csdl.RandomPrimitiveType(), false),
                                    Any.Int(1, 2)))
                        {
                            entityType.Add(property);
                            key.Add(Any.Csdl.PropertyRef(property.GetAttribute("Name")));
                            keyCount++;
                        }
                    }));
                    foreach (
                        var property in
                            Any.Sequence(
                                i => Any.Csdl.Property(Any.Csdl.RandomPrimitiveType()),
                                Any.Int(1, 3)))
                    {
                        entityType.Add(property);
                        propertyCount++;
                    }
                }));
                schema.Add(Any.Csdl.EntityType(schemaNamespace + "." + baseTypeName, entityType =>
                {
                    entityTypeName = entityType.GetAttribute("Name");
                    foreach (
                        var property in
                            Any.Sequence(
                                i => Any.Csdl.Property(Any.Csdl.RandomPrimitiveType()),
                                Any.Int(1, 3)))
                    {
                        entityType.Add(property);
                        propertyCount++;
                    }
                }));
                schema.Add(Any.Csdl.EntityContainer());
            });

            var serviceMetadata = new Dictionary<string, string>()
            {
                {"$metadata", edmxElement.ToString()}
            };
            var odcmModel = _odcmReader.GenerateOdcmModel(serviceMetadata);

            OdcmType odcmBaseType;
            odcmModel.TryResolveType(baseTypeName, schemaNamespace, out odcmBaseType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmBaseType
                .Should()
                .BeOfType<OdcmEntityClass>("because entity types should result in an OdcmClass");
            OdcmType odcmEntityType;
            odcmModel.TryResolveType(entityTypeName, schemaNamespace, out odcmEntityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmEntityType
                .Should()
                .BeOfType<OdcmEntityClass>("because entity types should result in an OdcmClass");
            odcmEntityType.As<OdcmEntityClass>().Base
                .Should()
                .Be(odcmBaseType,
                    "because an entity type with a base type set should have a corresponding OdcmClass and base OdcmClass");
            odcmBaseType.As<OdcmEntityClass>().Derived
                .Should()
                .Contain(odcmEntityType.As<OdcmEntityClass>(),
                    "because an entity type with a base type set should have a correspond OdcmClass that derives from a base OdcmClass");
        }

        [Fact]
        public void When_an_action_is_bound_to_an_entity_type_there_is_a_method_on_the_OdcmClass()
        {
            var entityTypeName = string.Empty;
            var schemaNamespace = string.Empty;
            var propertyCount = 0;
            var keyCount = 0;
            var parameterCount = 0;
            var actionName = string.Empty;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schemaNamespace = schema.GetAttribute("Namespace");
                schema.Add(Any.Csdl.EntityType(entityType =>
                {
                    entityTypeName = entityType.GetAttribute("Name");
                    entityType.Add(Any.Csdl.Key(key =>
                    {
                        foreach (
                            var property in
                                Any.Sequence(
                                    i => Any.Csdl.Property(Any.Csdl.RandomPrimitiveType(), false),
                                    Any.Int(1, 2)))
                        {
                            entityType.Add(property);
                            key.Add(Any.Csdl.PropertyRef(property.GetAttribute("Name")));
                            keyCount++;
                        }
                    }));
                    foreach (
                        var property in
                            Any.Sequence(
                                i => Any.Csdl.Property(Any.Csdl.RandomPrimitiveType()),
                                Any.Int(1, 3)))
                    {
                        entityType.Add(property);
                        propertyCount++;
                    }
                }));
                schema.Add(Any.Csdl.Action(Any.Csdl.RandomPrimitiveType(), action =>
                {
                    actionName = action.GetAttribute("Name");
                    action.AddAttribute("IsBound", true);
                    action.Add(Any.Csdl.Parameter(schemaNamespace + "." + entityTypeName));
                    foreach (
                        var parameter in
                            Any.Sequence(i => Any.Csdl.Parameter(Any.Csdl.RandomPrimitiveType()), Any.Int(1, 3)))
                    {
                        action.Add(parameter);
                        parameterCount++;
                    }
                }));
                schema.Add(Any.Csdl.EntityContainer());
            });

            var serviceMetadata = new Dictionary<string, string>()
            {
                {"$metadata", edmxElement.ToString()}
            };
            var odcmModel = _odcmReader.GenerateOdcmModel(serviceMetadata);

            OdcmType odcmEntityType;
            odcmModel.TryResolveType(entityTypeName, schemaNamespace, out odcmEntityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmEntityType
                .Should()
                .BeOfType<OdcmEntityClass>("because entity types should result in an OdcmClass");
            odcmEntityType.As<OdcmEntityClass>().Methods
                .Should()
                .Contain(odcmMethod => odcmMethod.Name == actionName, "because an action bound to an entity type should result in a method in the OdcmClass");
            odcmEntityType.As<OdcmEntityClass>()
                .Methods.Find(odcmMethod => odcmMethod.Name == actionName)
                .Parameters.Count
                .Should()
                .Be(parameterCount,
                    "because for each parameter in the action there should be a OdcmParameter in the OdcmMethod");
        }

        [Fact]
        public void When_a_function_is_bound_to_an_entity_type_there_is_a_method_on_the_OdcmClass()
        {
            var entityTypeName = string.Empty;
            var schemaNamespace = string.Empty;
            var propertyCount = 0;
            var keyCount = 0;
            var parameterCount = 0;
            var functionName = string.Empty;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schemaNamespace = schema.GetAttribute("Namespace");
                schema.Add(Any.Csdl.EntityType(entityType =>
                {
                    entityTypeName = entityType.GetAttribute("Name");
                    entityType.Add(Any.Csdl.Key(key =>
                    {
                        foreach (
                            var property in
                                Any.Sequence(
                                    i => Any.Csdl.Property(Any.Csdl.RandomPrimitiveType(), false),
                                    Any.Int(1, 2)))
                        {
                            entityType.Add(property);
                            key.Add(Any.Csdl.PropertyRef(property.GetAttribute("Name")));
                            keyCount++;
                        }
                    }));
                    foreach (
                        var property in
                            Any.Sequence(
                                i => Any.Csdl.Property(Any.Csdl.RandomPrimitiveType()),
                                Any.Int(1, 3)))
                    {
                        entityType.Add(property);
                        propertyCount++;
                    }
                }));
                schema.Add(Any.Csdl.Function(Any.Csdl.RandomPrimitiveType(), function =>
                {
                    functionName = function.GetAttribute("Name");
                    function.AddAttribute("IsBound", true);
                    function.Add(Any.Csdl.Parameter(schemaNamespace + "." + entityTypeName));
                    foreach (
                        var parameter in
                            Any.Sequence(i => Any.Csdl.Parameter(Any.Csdl.RandomPrimitiveType()), Any.Int(1, 3)))
                    {
                        function.Add(parameter);
                        parameterCount++;
                    }
                }));
                schema.Add(Any.Csdl.EntityContainer());
            });

            var serviceMetadata = new Dictionary<string, string>()
            {
                {"$metadata", edmxElement.ToString()}
            };
            var odcmModel = _odcmReader.GenerateOdcmModel(serviceMetadata);

            OdcmType odcmEntityType;
            odcmModel.TryResolveType(entityTypeName, schemaNamespace, out odcmEntityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmEntityType
                .Should()
                .BeOfType<OdcmEntityClass>("because entity types should result in an OdcmClass");
            odcmEntityType.As<OdcmEntityClass>().Methods
                .Should()
                .Contain(odcmMethod => odcmMethod.Name == functionName, "because a function bound to an entity type should result in a method in the OdcmClass");
            odcmEntityType.As<OdcmEntityClass>()
                .Methods.Find(odcmMethod => odcmMethod.Name == functionName)
                .Parameters.Count
                .Should()
                .Be(parameterCount,
                    "because for each parameter in the action there should be a OdcmParameter in the OdcmMethod");
        }
    }
}
