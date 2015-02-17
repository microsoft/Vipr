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
    public class Given_complex_types_in_a_valid_edmx_when_passed_to_the_ODataReader
    {
        private ODataReader.v4.OdcmReader _odcmReader;

        public Given_complex_types_in_a_valid_edmx_when_passed_to_the_ODataReader()
        {
            _odcmReader = new OdcmReader();
        }

        [Fact]
        public void It_returns_one_OdcmClass_for_each_ComplexType()
        {
            var complexTypeName = string.Empty;
            var schemaNamespace = string.Empty;
            var propertyCount = 0;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schema.Add(Any.Csdl.ComplexType(complexType =>
                {
                    foreach (
                        var property in
                            Any.Sequence(
                                i =>
                                    Any.Csdl.Property(
                                        property => property.AddAttribute("Type", Any.Csdl.RandomPrimitiveType())),
                                Any.Int(1, 5)))
                    {
                        complexType.Add(property);
                        propertyCount++;
                    }

                    complexTypeName = complexType.Attribute("Name").Value;
                }));
                schema.Add(Any.Csdl.EntityContainer());
                schemaNamespace = schema.Attribute("Namespace").Value;
            });

            var serviceMetadata = new Dictionary<string, string>()
            {
                {"$metadata", edmxElement.ToString()}
            };
            var odcmModel = _odcmReader.GenerateOdcmModel(serviceMetadata);

            OdcmType odcmComplexType;
            odcmModel.TryResolveType(complexTypeName, schemaNamespace, out odcmComplexType)
                .Should()
                .BeTrue("because a complex type in the schema should result in an OdcmType");
            odcmComplexType
                .Should()
                .BeOfType<OdcmClass>("because complex types should result in an OdcmClass");
            odcmComplexType.As<OdcmClass>().Properties.Count
                .Should()
                .Be(propertyCount, "because each property added to a complex type should result in an OdcmClass property");
        }

        [Fact]
        public void When_Abstract_is_set_it_returns_an_OdcmClass_with_IsAbstract_set()
        {
            var complexTypeName = string.Empty;
            var schemaNamespace = string.Empty;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schema.Add(Any.Csdl.ComplexType(complexType =>
                {
                    complexTypeName = complexType.Attribute("Name").Value;
                    complexType.AddAttribute("Abstract", true);
                }));
                schema.Add(Any.Csdl.EntityContainer());
                schemaNamespace = schema.Attribute("Namespace").Value;
            });

            var serviceMetadata = new Dictionary<string, string>()
            {
                {"$metadata", edmxElement.ToString()}
            };
            var odcmModel = _odcmReader.GenerateOdcmModel(serviceMetadata);

            OdcmType odcmComplexType;
            odcmModel.TryResolveType(complexTypeName, schemaNamespace, out odcmComplexType)
                .Should()
                .BeTrue("because a complex type in the schema should result in an OdcmType");
            odcmComplexType
                .Should()
                .BeOfType<OdcmClass>("because complex types should result in an OdcmClass");
            odcmComplexType.As<OdcmClass>().IsAbstract
                .Should()
                .BeTrue("because a complex type with the Abstract facet set should be abstract in the OdcmModel");
        }

        [Fact]
        public void When_OpenType_is_set_it_returns_an_OdcmClass_with_IsOpen_set()
        {
            var complexTypeName = string.Empty;
            var schemaNamespace = string.Empty;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schema.Add(Any.Csdl.ComplexType(complexType =>
                {
                    complexTypeName = complexType.Attribute("Name").Value;
                    complexType.AddAttribute("OpenType", true);
                }));
                schema.Add(Any.Csdl.EntityContainer());
                schemaNamespace = schema.Attribute("Namespace").Value;
            });

            var serviceMetadata = new Dictionary<string, string>()
            {
                {"$metadata", edmxElement.ToString()}
            };
            var odcmModel = _odcmReader.GenerateOdcmModel(serviceMetadata);

            OdcmType odcmComplexType;
            odcmModel.TryResolveType(complexTypeName, schemaNamespace, out odcmComplexType)
                .Should()
                .BeTrue("because a complex type in the schema should result in an OdcmType");
            odcmComplexType
                .Should()
                .BeOfType<OdcmClass>("because complex types should result in an OdcmClass");
            odcmComplexType.As<OdcmClass>().IsOpen
                .Should()
                .BeTrue("because a complex type with the OpenType facet set should be open in the OdcmModel");
        }

        [Fact]
        public void When_BaseType_is_set_it_returns_an_OdcmClass_with_a_BaseType_set()
        {
            var baseTypeName = string.Empty;
            var complexTypeName = string.Empty;
            var schemaNamespace = string.Empty;

            var edmxElement = Any.Csdl.EdmxToSchema(schema =>
            {
                schemaNamespace = schema.Attribute("Namespace").Value;
                schema.Add(Any.Csdl.ComplexType(complexType =>
                {
                    baseTypeName = complexType.Attribute("Name").Value;
                    complexType.AddAttribute("Abstract", true);
                }));
                schema.Add(Any.Csdl.ComplexType(complexType =>
                {
                    complexTypeName = complexType.Attribute("Name").Value;
                    complexType.AddAttribute("BaseType", schemaNamespace + "." + baseTypeName);
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
                .BeTrue("because a complex type in the schema should result in an OdcmType");
            odcmBaseType
                .Should()
                .BeOfType<OdcmClass>("because complex types should result in an OdcmClass");
            OdcmType odcmComplexType;
            odcmModel.TryResolveType(complexTypeName, schemaNamespace, out odcmComplexType)
                .Should()
                .BeTrue("because a complex type in the schema should result in an OdcmType");
            odcmComplexType
                .Should()
                .BeOfType<OdcmClass>("because complex types should result in an OdcmClass");
            odcmComplexType.As<OdcmClass>().Base
                .Should()
                .Be(odcmBaseType,
                    "because a complex type with a base type set should have a corresponding OdcmClass and base OdcmClass");
            odcmBaseType.As<OdcmClass>().Derived
                .Should()
                .Contain(odcmComplexType.As<OdcmClass>(),
                    "because a complex type with a base type set should have a correspond OdcmClass that derives from a base OdcmClass");
        }
    }
}
