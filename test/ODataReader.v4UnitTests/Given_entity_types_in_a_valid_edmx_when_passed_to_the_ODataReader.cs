// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Its.Recipes;
using ODataReader.v4;
using System.Collections.Generic;
using Vipr.Core.CodeModel;
using Xunit;

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
        public void It_returns_one_OdcmClass_for_each_EntityType()
        {
            var entityTypeName = string.Empty;
            var schemaNamespace = string.Empty;
            var propertyCount = 0;
            var keyCount = 0;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schema.Add(Any.Csdl.EntityType(entityType =>
                {
                    entityType.Add(Any.Csdl.Key(key =>
                    {
                        foreach (
                            var property in
                                Any.Sequence(
                                    i => Any.Csdl.Property(Any.Csdl.RandomPrimitiveType()),
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

                    entityTypeName = entityType.Attribute("Name").Value;
                }));
                schema.Add(Any.Csdl.EntityContainer());
                schemaNamespace = schema.Attribute("Namespace").Value;
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
                .BeOfType<OdcmClass>("because entity types should result in an OdcmClass");
            odcmEntityType.As<OdcmClass>().Key.Count
                .Should()
                .Be(keyCount, "because each property reference added to the key of an entity type should result in an OdcmClass property");
            odcmEntityType.As<OdcmClass>().Properties.Count
                .Should()
                .Be(propertyCount + keyCount, "because each property added to an entity type should result in an OdcmClass property");
            odcmEntityType.As<OdcmClass>().IsAbstract
                .Should()
                .BeFalse("because an entity type with no Abstract facet should be false in the OdcmModel");
            odcmEntityType.As<OdcmClass>().IsOpen
                .Should()
                .BeFalse("because an entity type with no OpenType facet should be false in the OdcmModel");
        }

        [Fact]
        public void When_Abstract_is_set_it_returns_an_OdcmClass_with_IsAbstract_set()
        {
            var entityTypeName = string.Empty;
            var schemaNamespace = string.Empty;
            var propertyCount = 0;
            var keyCount = 0;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schema.Add(Any.Csdl.EntityType(entityType =>
                {
                    entityType.Add(Any.Csdl.Key(key =>
                    {
                        foreach (
                            var property in
                                Any.Sequence(
                                    i => Any.Csdl.Property(Any.Csdl.RandomPrimitiveType()),
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

                    entityTypeName = entityType.Attribute("Name").Value;
                    entityType.AddAttribute("Abstract", true);
                }));
                schema.Add(Any.Csdl.EntityContainer());
                schemaNamespace = schema.Attribute("Namespace").Value;
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
                .BeOfType<OdcmClass>("because entity types should result in an OdcmClass");
            odcmEntityType.As<OdcmClass>().IsAbstract
                .Should()
                .BeTrue("because an entity type with the Abstract facet set should be abstract in the OdcmModel");
        }

        [Fact]
        public void When_OpenType_is_set_it_returns_an_OdcmClass_with_IsOpen_set()
        {
            var entityTypeName = string.Empty;
            var schemaNamespace = string.Empty;
            var propertyCount = 0;
            var keyCount = 0;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schema.Add(Any.Csdl.EntityType(entityType =>
                {
                    entityType.Add(Any.Csdl.Key(key =>
                    {
                        foreach (
                            var property in
                                Any.Sequence(
                                    i => Any.Csdl.Property(Any.Csdl.RandomPrimitiveType()),
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

                    entityTypeName = entityType.Attribute("Name").Value;
                    entityType.AddAttribute("OpenType", true);
                }));
                schema.Add(Any.Csdl.EntityContainer());
                schemaNamespace = schema.Attribute("Namespace").Value;
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
                .BeOfType<OdcmClass>("because entity types should result in an OdcmClass");
            odcmEntityType.As<OdcmClass>().IsOpen
                .Should()
                .BeTrue("because an entity type with the OpenType facet set should be open in the OdcmModel");
        }

        [Fact]
        public void When_HasStream_is_set_it_returns_an_OdcmClass_with_MediaEntity_kind()
        {
            var entityTypeName = string.Empty;
            var schemaNamespace = string.Empty;
            var propertyCount = 0;
            var keyCount = 0;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schema.Add(Any.Csdl.EntityType(entityType =>
                {
                    entityType.Add(Any.Csdl.Key(key =>
                    {
                        foreach (
                            var property in
                                Any.Sequence(
                                    i => Any.Csdl.Property(Any.Csdl.RandomPrimitiveType()),
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

                    entityTypeName = entityType.Attribute("Name").Value;
                    entityType.AddAttribute("HasStream", true);
                }));
                schema.Add(Any.Csdl.EntityContainer());
                schemaNamespace = schema.Attribute("Namespace").Value;
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
                .BeOfType<OdcmClass>("because entity types should result in an OdcmClass");
            odcmEntityType.As<OdcmClass>().Kind
                .Should()
                .Be(OdcmClassKind.MediaEntity, "because an entity type with the HasStream facet set should be a media entity in the OdcmModel");
        }

        [Fact]
        public void When_BaseType_is_set_it_returns_an_OdcmClass_with_a_BaseType_set()
        {
            var baseTypeName = string.Empty;
            var entityTypeName = string.Empty;
            var schemaNamespace = string.Empty;
            var propertyCount = 0;
            var keyCount = 0;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schemaNamespace = schema.Attribute("Namespace").Value;
                schema.Add(Any.Csdl.EntityType(entityType =>
                {
                    entityType.Add(Any.Csdl.Key(key =>
                    {
                        foreach (
                            var property in
                                Any.Sequence(
                                    i => Any.Csdl.Property(Any.Csdl.RandomPrimitiveType()),
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

                    baseTypeName = entityType.Attribute("Name").Value;
                    entityType.AddAttribute("Abstract", true);
                }));
                schema.Add(Any.Csdl.EntityType(entityType =>
                {
                    foreach (
                        var property in
                            Any.Sequence(
                                i => Any.Csdl.Property(Any.Csdl.RandomPrimitiveType()),
                                Any.Int(1, 3)))
                    {
                        entityType.Add(property);
                        propertyCount++;
                    }

                    entityTypeName = entityType.Attribute("Name").Value;
                    entityType.AddAttribute("BaseType", schemaNamespace + "." + baseTypeName);
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
                .BeOfType<OdcmClass>("because entity types should result in an OdcmClass");
            OdcmType odcmEntityType;
            odcmModel.TryResolveType(entityTypeName, schemaNamespace, out odcmEntityType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");
            odcmEntityType
                .Should()
                .BeOfType<OdcmClass>("because entity types should result in an OdcmClass");
            odcmEntityType.As<OdcmClass>().Base
                .Should()
                .Be(odcmBaseType,
                    "because an entity type with a base type set should have a corresponding OdcmClass and base OdcmClass");
            odcmBaseType.As<OdcmClass>().Derived
                .Should()
                .Contain(odcmEntityType.As<OdcmClass>(),
                    "because an entity type with a base type set should have a correspond OdcmClass that derives from a base OdcmClass");
        }
    }
}
